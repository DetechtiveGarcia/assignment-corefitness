using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class MemberRepository(PersistenceContext context) : IMemberRepository
{
    public async Task<bool> AddAsync(Member member, CancellationToken ct = default)
    {
        await context.AddAsync(member, ct);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpdateAsync(Member member, CancellationToken ct = default)
    {
        context.Update(member);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveAsync(Member member, CancellationToken ct = default)
    {
        context.Remove(member);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<Member?> GetAsync(Expression<Func<Member, bool>> expression, CancellationToken ct = default)
    {
        var member = await context.Members//.Include(x => x.Memberships)
            .FirstOrDefaultAsync(expression, ct);
        
        return member;
    }


}