using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class MembershipRepository(PersistenceContext context) : IMembershipRepository
{
    public async Task<bool> AddAsync(Membership membership, CancellationToken ct = default)
    {
        await context.Memberships.AddAsync(membership, ct);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<Membership?> GetAsync(Expression<Func<Membership, bool>> expression, CancellationToken ct = default)
    {
        return await context.Memberships.FirstOrDefaultAsync(expression, ct);
    }
}