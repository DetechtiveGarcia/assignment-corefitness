using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Dtos.Memberships;
using Application.Dtos.Memberships.Requests;
using Domain.Entities;

namespace Application.Services.Memberships;

public class MembershipService(
    IMembershipRepository membershipRepository,
    IMemberRepository memberRepository) : IMembershipService
{
    public async Task<bool> CreateMembershipAsync(CreateMembershipRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return false;

        if (string.IsNullOrWhiteSpace(request.UserId))
            return false;

        if (string.IsNullOrWhiteSpace(request.MembershipName))
            return false;

        var member = await memberRepository.GetAsync(x => x.UserId == request.UserId, ct);
        if (member is null)
            return false;

        var existingMembership = await membershipRepository.GetAsync(x => x.MemberId == member.Id, ct);
        if (existingMembership is not null)
            return false;

        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(1);

        var membership = Membership.Create(
            member.Id,
            request.MembershipName,
            startDate,
            endDate
        );

        return await membershipRepository.AddAsync(membership, ct);
    }

    public async Task<MembershipDetails?> GetMembershipAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var member = await memberRepository.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
            return null;

        var membership = await membershipRepository.GetAsync(x => x.MemberId == member.Id, ct);
        if (membership is null)
            return null;

        return new MembershipDetails(
            membership.Id,
            membership.MembershipName,
            membership.StartDate,
            membership.EndDate
        );
    }
}