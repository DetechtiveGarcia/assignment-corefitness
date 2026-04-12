namespace Application.Dtos.Members.Requests;

public sealed record UpdateMemberRequest
(
    string Id, string FirstName,
    string LastName,
    string? PhoneNumber,
    string? ProfileImageUrl
);