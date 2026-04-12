using Application.Dtos.FitnessClasses.Requests;
using Application.Dtos.FitnessClasses.Results;

namespace Application.Abstractions.Services;

public interface IFitnessClassService
{
    Task<FitnessClassResult> CreateAsync(CreateFitnessClassRequest request, CancellationToken ct = default);
    Task<FitnessClassListResult> GetAllAsync(CancellationToken ct = default);
    Task<bool> DeleteAsync(string id, CancellationToken ct = default);
}