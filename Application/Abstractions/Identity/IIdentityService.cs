using Application.Dtos.Identity.Results;

namespace Application.Abstractions.Identity;
public interface IIdentityService
{
    Task<RegisterResult> RegisterAsync(string email, string password, string roleName);
    Task<bool> LoginAsync(string email, string password, bool rememberMe, bool lockoutOnFailure = false);
    Task LogoutAsync();
    Task<string?> GetEmailAsync(string userId);
    Task<string?> GetPhoneNumberAsync(string userId);

    Task<bool> DeleteAsync(string userId);
    Task<bool> FindExistingAsync(string email);
    Task<bool> UpdatePhoneNumberAsync(string userId, string phoneNumber);
}
