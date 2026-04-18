using Application.Dtos.ClassBookings;

namespace Application.Dtos.ClassBookings.Results;

public sealed record ClassBookingListResult(
    bool Succeeded,
    IReadOnlyCollection<string> Errors,
    IEnumerable<ClassBookingDetails>? Bookings = null
);