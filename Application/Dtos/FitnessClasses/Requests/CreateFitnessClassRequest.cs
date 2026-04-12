namespace Application.Dtos.FitnessClasses.Requests;

public sealed record CreateFitnessClassRequest(
    string Title,
    string? Description,
    string? InstructorName,
    string? Category,
    DateTime StartTime,
    DateTime EndTime
);