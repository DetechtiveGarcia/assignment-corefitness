namespace Application.Dtos.Members.Results;

public sealed record MemberResult(bool Succeeded, IReadOnlyCollection<string> Errors, string? Id = null, string? UserId = null);