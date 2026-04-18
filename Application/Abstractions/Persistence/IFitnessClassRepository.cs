using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Persistence;

public interface IFitnessClassRepository
{
    Task<bool> AddAsync(FitnessClass fitnessClass, CancellationToken ct = default);
    Task<bool> UpdateAsync(FitnessClass fitnessClass, CancellationToken ct = default);
    Task<bool> RemoveAsync(FitnessClass fitnessClass, CancellationToken ct = default);
    Task<FitnessClass?> GetAsync(Expression<Func<FitnessClass, bool>> expression, CancellationToken ct = default);
    Task<IEnumerable<FitnessClass>> GetAllAsync(CancellationToken ct = default);
}