using Application.Dtos.ClassBookings.Results;

namespace Application.Abstractions.Services;

public interface IClassBookingService
{
    Task<ClassBookingResult> BookAsync(string userId, string fitnessClassId, CancellationToken ct = default);
    Task<ClassBookingResult> CancelAsync(string userId, string fitnessClassId, CancellationToken ct = default);
    Task<ClassBookingListResult> GetMyBookingsAsync(string userId, CancellationToken ct = default);
}