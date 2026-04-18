using Application.Abstractions.Identity;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Dtos.Members;
using Application.Dtos.Members.Requests;
using Application.Dtos.Members.Results;
using Domain.Entities;

namespace Application.Services.Members;

public class MemberService(IIdentityService identityService, IMemberRepository repo) : IMemberService
{
    public async Task<MemberResult> CreateMemberAsync(RegisterMemberRequest request, CancellationToken ct = default)
    {
        var errors = new List<string>();
        if (request is null)
        {
            errors.Add("request model must be provided.");
            return new MemberResult(false, errors);
        }

        var existing = await identityService.FindExistingAsync(request.Email);
        if (existing)
        {
            errors.Add("An account with the same email already exists.");
            return new MemberResult(false, errors);
        }

        var registerResult = await identityService.RegisterAsync(request.Email, request.Password, "Member");
        if (!registerResult.Succeeded)
            return new MemberResult(false, registerResult.Errors);

        if (string.IsNullOrWhiteSpace(registerResult.UserId))
        {
            errors.Add("User Id is missing");
            return new MemberResult(false, errors);
        }
        var member = Member.Create(registerResult.UserId);

        await repo.AddAsync(member, ct);

        return new MemberResult(true, [], member.Id, member.UserId);

    }


    public async Task<MemberResult> UpdateMemberAsync(UpdateMemberRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return new MemberResult(false, ["request model must be provided"]);

        var member = await repo.GetAsync(x => x.Id == request.Id, ct);
        if (member is null)
            return new MemberResult(false, ["Id is missing"]);

        if (member.UserId is null)
            return new MemberResult(false, ["Member UserId is missing"]);


        member.UpdateProfileInformation(request.FirstName, request.LastName, request.ProfileImageUrl);

        var memberUpdated = await repo.UpdateAsync(member, ct);
        if (!memberUpdated)
            return new MemberResult(false, ["Unable to update account"]);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            bool phoneNumberUpdated = await identityService.UpdatePhoneNumberAsync(member.UserId, request.PhoneNumber);
            if (!phoneNumberUpdated)
                return new MemberResult(true, ["Member details updated but not phone number."]);
        }

        return new MemberResult(true, []);
    }
    public async Task<MemberDetailsResult> GetMemberDetailAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return new MemberDetailsResult(false, ["Id is missing"]);

        var member = await repo.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
            return new MemberDetailsResult(false, ["Id is missing"]);
        if (member.UserId is null)
            return new MemberDetailsResult(false, ["Member userId is missing"]);

        var email = await identityService.GetEmailAsync(member.UserId);
        var phoneNumber = await identityService.GetPhoneNumberAsync(member.UserId);

        var memberDetails = new MemberDetails(member.Id, member.UserId, member.FirstName, member.LastName, email, phoneNumber, member.ProfileImageUrl);

        return new MemberDetailsResult(true, [], memberDetails);
    }

    public async Task<MemberResult> DeleteMemberAsync(string userId, CancellationToken ct = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userId))
        {
            errors.Add("User Id is missing");
            return new MemberResult(false, errors);
        }
        var member = await repo.GetAsync(x => x.UserId == userId, ct);
        if (member is null)
        {
            errors.Add("Member not found");
            return new MemberResult(false, errors);
        }

        if (member.UserId is null)
        {
            errors.Add("Member UserId not found");
            return new MemberResult(false, errors);
        }
        await repo.RemoveAsync(member, ct);

        var deleted = await identityService.DeleteAsync(member.UserId);


        return deleted ? new MemberResult(deleted, errors) : new MemberResult(deleted, ["Unable to delete account"]);
    }

}