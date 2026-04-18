namespace Application.Dtos.FitnessClasses.Results;

public sealed record FitnessClassResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    string? Id = null
);