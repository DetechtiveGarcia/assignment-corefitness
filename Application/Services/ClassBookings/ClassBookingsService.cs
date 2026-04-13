using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Dtos.ClassBookings;
using Application.Dtos.ClassBookings.Results;
using Domain.Entities;

namespace Application.Services.ClassBookings;

public class ClassBookingService(
    IClassBookingRepository bookingRepository,
    IMemberRepository memberRepository,
    IFitnessClassRepository fitnessClassRepository) : IClassBookingService
{
    public async Task<ClassBookingResult> BookAsync(string userId, string fitnessClassId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return new ClassBookingResult(false, ["User id is missing."]);

        if (string.IsNullOrWhiteSpace(fitnessClassId))
            return new ClassBookingResult(false, ["Class id is missing."]);

        var member = await memberRepository.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
            return new ClassBookingResult(false, ["Member not found."]);

        var fitnessClass = await fitnessClassRepository.GetAsync(x => x.Id == fitnessClassId, ct);
        if (fitnessClass is null)
            return new ClassBookingResult(false, ["Class not found."]);

        var existingBooking = await bookingRepository.GetAsync(
            x => x.MemberId == member.Id && x.FitnessClassId == fitnessClassId, ct);

        if (existingBooking is not null)
            return new ClassBookingResult(false, ["You have already booked this class."]);

        var booking = ClassBooking.Create(member.Id, fitnessClassId);
        await bookingRepository.AddAsync(booking, ct);

        return new ClassBookingResult(true, []);
    }

    public async Task<ClassBookingResult> CancelAsync(string userId, string fitnessClassId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return new ClassBookingResult(false, ["User id is missing."]);

        if (string.IsNullOrWhiteSpace(fitnessClassId))
            return new ClassBookingResult(false, ["Class id is missing."]);

        var member = await memberRepository.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
            return new ClassBookingResult(false, ["Member not found."]);

        var booking = await bookingRepository.GetAsync(
            x => x.MemberId == member.Id && x.FitnessClassId == fitnessClassId, ct);

        if (booking is null)
            return new ClassBookingResult(false, ["Booking not found."]);

        await bookingRepository.RemoveAsync(booking, ct);

        return new ClassBookingResult(true, []);
    }

    public async Task<ClassBookingListResult> GetMyBookingsAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return new ClassBookingListResult(false, ["User id is missing."]);

        var member = await memberRepository.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
            return new ClassBookingListResult(false, ["Member not found."]);

        var bookings = await bookingRepository.GetAllAsync(x => x.MemberId == member.Id, ct);

        var result = new List<ClassBookingDetails>();

        foreach (var booking in bookings)
        {
            var fitnessClass = await fitnessClassRepository.GetAsync(x => x.Id == booking.FitnessClassId, ct);
            if (fitnessClass is null)
                continue;

            result.Add(new ClassBookingDetails(
                booking.Id,
                fitnessClass.Id,
                fitnessClass.Title,
                fitnessClass.Description,
                fitnessClass.InstructorName,
                fitnessClass.Category,
                fitnessClass.StartTime,
                fitnessClass.EndTime,
                booking.BookedAt
            ));
        }

        return new ClassBookingListResult(true, [], result);
    }
}