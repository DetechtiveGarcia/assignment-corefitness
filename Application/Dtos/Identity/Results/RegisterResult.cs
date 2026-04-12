namespace Application.Dtos.Identity.Results;

public sealed record RegisterResult(bool Succeeded, IReadOnlyCollection<string> Errors, string? UserId = null);