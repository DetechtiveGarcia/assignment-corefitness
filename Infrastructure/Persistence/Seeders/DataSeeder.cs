using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Member", "Admin"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task SeedAdminAsync(UserManager<AppUser> userManager)
    {
        var email = "admin@corefitness.com";
        var password = "Admin123!";

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return;

        var adminUser = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}