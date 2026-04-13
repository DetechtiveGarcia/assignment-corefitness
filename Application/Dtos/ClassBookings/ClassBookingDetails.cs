namespace Application.Dtos.ClassBookings;

public sealed record ClassBookingDetails(
    string BookingId,
    string FitnessClassId,
    string Title,
    string? Description,
    string? InstructorName,
    string? Category,
    DateTime StartTime,
    DateTime EndTime,
    DateTime BookedAt
);