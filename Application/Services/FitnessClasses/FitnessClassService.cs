using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Dtos.FitnessClasses;
using Application.Dtos.FitnessClasses.Requests;
using Application.Dtos.FitnessClasses.Results;
using Domain.Entities;

namespace Application.Services.FitnessClasses;

public class FitnessClassService(IFitnessClassRepository repository) : IFitnessClassService
{
    public async Task<FitnessClassResult> CreateAsync(CreateFitnessClassRequest request, CancellationToken ct = default)
    {
        var errors = new List<string>();

        if (request is null)
            return new FitnessClassResult(false, ["Request must be provided."]);

        if (string.IsNullOrWhiteSpace(request.Title))
            errors.Add("Title is required.");

        if (request.EndTime <= request.StartTime)
            errors.Add("End time must be after start time.");

        if (errors.Count > 0)
            return new FitnessClassResult(false, errors);

        var fitnessClass = FitnessClass.Create(
            request.Title,
            request.Description,
            request.InstructorName,
            request.Category,
            request.StartTime,
            request.EndTime
        );

        await repository.AddAsync(fitnessClass, ct);

        return new FitnessClassResult(true, [], fitnessClass.Id);
    }

    public async Task<FitnessClassListResult> GetAllAsync(CancellationToken ct = default)
    {
        var classes = await repository.GetAllAsync(ct);

        var result = classes.Select(x => new FitnessClassDetails(
            x.Id,
            x.Title,
            x.Description,
            x.InstructorName,
            x.Category,
            x.StartTime,
            x.EndTime
        ));

        return new FitnessClassListResult(true, [], result);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct = default)
    {
        var fitnessClass = await repository.GetAsync(x => x.Id == id, ct);

        if (fitnessClass is null)
            return false;

        await repository.RemoveAsync(fitnessClass, ct);
        return true;
    }
}