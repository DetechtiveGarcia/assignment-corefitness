using Application.Dtos.Memberships;

namespace Application.Dtos.Members;

public record MemberDetails
(
    string Id,
    string UserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    string? ProfileImageUrl,
    MembershipDetails? Membership = null
);