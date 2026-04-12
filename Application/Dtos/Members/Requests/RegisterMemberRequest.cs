namespace Application.Dtos.Members.Requests;

public record RegisterMemberRequest
(
    string Email,
    string Password,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ProfileImageUrl
);