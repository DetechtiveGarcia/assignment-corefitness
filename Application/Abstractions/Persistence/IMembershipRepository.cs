using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Persistence;

public interface IMembershipRepository
{
    Task<bool> AddAsync(Membership membership, CancellationToken ct = default);
    Task<Membership?> GetAsync(Expression<Func<Membership, bool>> expression, CancellationToken ct = default);
}