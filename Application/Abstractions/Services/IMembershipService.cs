using Application.Dtos.Memberships;
using Application.Dtos.Memberships.Requests;

namespace Application.Abstractions.Services;

public interface IMembershipService
{
    Task<bool> CreateMembershipAsync(CreateMembershipRequest request, CancellationToken ct = default);
    Task<MembershipDetails?> GetMembershipAsync(string userId, CancellationToken ct = default);
}