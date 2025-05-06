using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;
using System.Threading.Tasks;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;
        private readonly IMapper _mapper;
        public CategoryController(ShopAppContext shopAppContext,IMapper mapper)
        {
            _shopAppContext = shopAppContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return BadRequest();
            var existCategory = await _shopAppContext.Categories
                .Include(c=>c.Products)
                .Where(c => !c.IsDelete)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existCategory == null) return NotFound();
            var productReturnDto = _mapper.Map<CategoryReturnDto>(existCategory);
            return Ok(productReturnDto);
        }

        private CategoryReturnDto MapToReturnDto(Category existCategory)
        {
            
            CategoryReturnDto categoryReturnDto = new();
            categoryReturnDto.Id = existCategory.Id;
            categoryReturnDto.Name = existCategory.Name;
            categoryReturnDto.CreatedDate = existCategory.CreatedDate;
            categoryReturnDto.UpdateDate = existCategory.UpdateDate;
            categoryReturnDto.ImageUrl ="http://localhost:5093/images/" + existCategory.Image;
            return categoryReturnDto;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var isExist = await _shopAppContext.Categories
                .AnyAsync(c => !c.IsDelete && c.Name.ToLower() == categoryCreateDto.Name.ToLower());
            if (isExist)
            {
                return StatusCode(409);
            }
            Category category = new();
            if(categoryCreateDto.Photo is null)
            {
                return BadRequest();
            }
            if (!categoryCreateDto.Photo.ContentType.Contains("image/"))
            {
                return BadRequest();
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryCreateDto.Photo.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryCreateDto.Photo.CopyToAsync(fileStream);
            category.Name = categoryCreateDto.Name.Trim();
            category.Image =fileName;
            await _shopAppContext.Categories.AddAsync(category);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(201);
        }
    }
}
