namespace Application.Dtos.ClassBookings.Results;

public sealed record ClassBookingResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors
);