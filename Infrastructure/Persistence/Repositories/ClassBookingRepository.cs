using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ClassBookingRepository(PersistenceContext context) : IClassBookingRepository
{
    public async Task<bool> AddAsync(ClassBooking booking, CancellationToken ct = default)
    {
        await context.ClassBookings.AddAsync(booking, ct);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveAsync(ClassBooking booking, CancellationToken ct = default)
    {
        context.ClassBookings.Remove(booking);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<ClassBooking?> GetAsync(Expression<Func<ClassBooking, bool>> expression, CancellationToken ct = default)
    {
        return await context.ClassBookings.FirstOrDefaultAsync(expression, ct);
    }

    public async Task<IEnumerable<ClassBooking>> GetAllAsync(Expression<Func<ClassBooking, bool>> expression, CancellationToken ct = default)
    {
        return await context.ClassBookings
            .Where(expression)
            .OrderByDescending(x => x.BookedAt)
            .ToListAsync(ct);
    }
}