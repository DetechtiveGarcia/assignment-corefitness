using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Persistence;
public interface IMemberRepository
{
    Task<bool> AddAsync(Member member, CancellationToken ct = default);
    Task<bool> UpdateAsync(Member member, CancellationToken ct = default);
    Task<bool> RemoveAsync(Member member, CancellationToken ct = default);
    Task<Member?> GetAsync(Expression<Func<Member, bool>> expression, CancellationToken ct = default);
}
