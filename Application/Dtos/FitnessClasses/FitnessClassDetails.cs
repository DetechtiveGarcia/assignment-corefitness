namespace Application.Dtos.FitnessClasses;

public sealed record FitnessClassDetails(
    string Id,
    string Title,
    string? Description,
    string? InstructorName,
    string? Category,
    DateTime StartTime,
    DateTime EndTime
);