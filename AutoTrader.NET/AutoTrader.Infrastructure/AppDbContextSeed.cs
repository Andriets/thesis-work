using AutoTrader.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AutoTrader.Infrastructure
{
    public class AppDbContextSeed
    {
        public static async Task SeedDefaultUsersAsync(UserManager<AppUser> userManager, RoleManager<Role> roleManager)
        {
            if (await userManager.FindByEmailAsync("user@gmail.com") is null)
            {
                var user = new AppUser
                {
                    UserName = "User",
                    Email = "user@gmail.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Welcome1");
            }
        }
    }
}
