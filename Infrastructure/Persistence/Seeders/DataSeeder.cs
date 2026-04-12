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
}