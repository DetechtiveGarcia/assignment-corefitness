using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Persistence;

public interface IClassBookingRepository
{
    Task<bool> AddAsync(ClassBooking booking, CancellationToken ct = default);
    Task<bool> RemoveAsync(ClassBooking booking, CancellationToken ct = default);
    Task<ClassBooking?> GetAsync(Expression<Func<ClassBooking, bool>> expression, CancellationToken ct = default);
    Task<IEnumerable<ClassBooking>> GetAllAsync(Expression<Func<ClassBooking, bool>> expression, CancellationToken ct = default);
}