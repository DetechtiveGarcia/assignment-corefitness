using Application.Abstractions.Identity;
using Application.Dtos.Identity.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services;

public class IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IIdentityService
{
    public async Task<RegisterResult> RegisterAsync(string email, string password, string roleName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        var user = new AppUser
        {
            UserName = email,
            Email = email
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return new RegisterResult(false, [.. createResult.Errors.Select(x => x.Description)]);

        var roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
            return new RegisterResult(false, [.. roleResult.Errors.Select(x => x.Description)]);

        return new RegisterResult(true, [], user.Id);

    }

    public async Task<bool> LoginAsync(string email, string password, bool rememberMe, bool lockoutOnFailure = false)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        var signInResult = await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure);

        return signInResult.Succeeded;
    }

    public Task LogoutAsync() => signInManager.SignOutAsync();

    public async Task<bool> FindExistingAsync(string email) => await userManager.Users.AnyAsync(x => x.Email == email);

    public async Task<string?> GetEmailAsync(string userId)
        => await userManager.Users.AsNoTracking().Where(x => x.Id == userId).Select(x => x.Email).FirstOrDefaultAsync();


    public async Task<string?> GetPhoneNumberAsync(string userId)
        => await userManager.Users.AsNoTracking().Where(x => x.Id == userId).Select(x => x.PhoneNumber).FirstOrDefaultAsync();

    public async Task<bool> UpdatePhoneNumberAsync(string userId, string phoneNumber)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;

        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        var result = await userManager.ChangePhoneNumberAsync(user, phoneNumber, token);

        return result.Succeeded;

    }
    public async Task<bool> DeleteAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        var deleteResult = await userManager.DeleteAsync(user);
        return deleteResult.Succeeded;

    }
}