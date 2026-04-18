using Application.Dtos.FitnessClasses;

namespace Application.Dtos.FitnessClasses.Results;

public sealed record FitnessClassListResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    IEnumerable<FitnessClassDetails>? FitnessClasses = null
);