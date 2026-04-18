namespace Application.Dtos.Members.Results;

public sealed record MemberDetailsResult(bool Succeeded, IReadOnlyCollection<string> Errors, MemberDetails? MemberDetails = null);