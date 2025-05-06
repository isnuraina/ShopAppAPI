using Microsoft.AspNetCore.Identity;

namespace ShopAppAPI.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
