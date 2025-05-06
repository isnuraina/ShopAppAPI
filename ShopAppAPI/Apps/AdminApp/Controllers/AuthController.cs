using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;
using ShopAppAPI.Entities;
using System.Threading.Tasks;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser =await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser!=null)
            {
                return BadRequest();
            }
            AppUser user = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FullName = registerDto.FullName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "member");
            return StatusCode(201);
        }

        public async Task<IActionResult> CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("member"))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "member" });
            }
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "admin" });
            }
            return StatusCode(201);
        }

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return BadRequest();
            var result =await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return BadRequest();
            var token = "";
            return Ok(new { token = "" });
        }
    }
}
