using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class FitnessClassRepository(PersistenceContext context) : IFitnessClassRepository
{
    public async Task<bool> AddAsync(FitnessClass fitnessClass, CancellationToken ct = default)
    {
        await context.FitnessClasses.AddAsync(fitnessClass, ct);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpdateAsync(FitnessClass fitnessClass, CancellationToken ct = default)
    {
        context.FitnessClasses.Update(fitnessClass);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveAsync(FitnessClass fitnessClass, CancellationToken ct = default)
    {
        context.FitnessClasses.Remove(fitnessClass);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<FitnessClass?> GetAsync(Expression<Func<FitnessClass, bool>> expression, CancellationToken ct = default)
    {
        return await context.FitnessClasses.FirstOrDefaultAsync(expression, ct);
    }

    public async Task<IEnumerable<FitnessClass>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.FitnessClasses
            .OrderBy(x => x.StartTime)
            .ToListAsync(ct);
    }
}