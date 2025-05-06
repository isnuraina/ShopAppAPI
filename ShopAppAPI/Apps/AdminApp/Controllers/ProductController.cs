using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;
using System.Threading.Tasks;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;
        private readonly IMapper _mapper;

        public ProductController(ShopAppContext shopAppContext,IMapper mapper)
        {
            _shopAppContext = shopAppContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _shopAppContext.Products
                .Include(p=>p.Category)
                .ThenInclude(c=>c.Products)
                .Where(p=>!p.IsDelete)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct == null) return NotFound();
            return Ok(_mapper.Map<ProductReturnDto>(existProduct));
        }
        [HttpGet]
        public async Task< IActionResult> Get(string search, int page = 1)
        {
            var query =  _shopAppContext.Products.Where(p => !p.IsDelete);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }
            ProductListDto productListDto = new();
            productListDto.Page = page;
            productListDto.TotalCount = query.Count();
            productListDto.Items =await query.Skip((page - 1) * 2).Take(2)
                .Select(p => new ProductListItemDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice,
                    CostPrice = p.CostPrice,
                    CreatedDate = p.CreatedDate,
                    UpdateDate = p.UpdateDate,
                    Category = new()
                    {
                        Name=p.Category.Name,
                        ProductCount=p.Category.Products.Count 
                    }
                })
                .ToListAsync();
            return Ok(productListDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            if (!await _shopAppContext.Categories.AnyAsync(c => !c.IsDelete && c.Id == productCreateDto.CategoryId))
            {
                return StatusCode(409);
            };

            Product product = new();
            product.Name = productCreateDto.Name;
            product.SalePrice = productCreateDto.SalePrice;
            product.CostPrice = productCreateDto.CostPrice;
            product.CategoryId = productCreateDto.CategoryId;
            await _shopAppContext.AddAsync(product);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id,ProductUpdateDto productUpdateDto)
        {
            if (id is null) return BadRequest();
            var existProduct =await _shopAppContext.Products
                .Where(p=>!p.IsDelete)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct is null) return NotFound();
            if (!await _shopAppContext.Categories.AnyAsync(c => !c.IsDelete && c.Id == productUpdateDto.CategoryId))
            {
                return StatusCode(409);
            };
            existProduct.Name = productUpdateDto.Name;
            existProduct.SalePrice = productUpdateDto.SalePrice;
            existProduct.CostPrice = productUpdateDto.CostPrice;
            existProduct.CategoryId = productUpdateDto.CategoryId;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id,bool status)
        {
            var existProduct = await _shopAppContext.Products
                .Where(p=>!p.IsDelete)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct is null) return NotFound();
            existProduct.IsDelete = status;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existProduct =await _shopAppContext.Products
                .Where(p=>!p.IsDelete)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct is null) return NotFound();
            _shopAppContext.Products.Remove(existProduct);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
