using Application.Dtos.Members.Requests;
using Application.Dtos.Members.Results;

namespace Application.Abstractions.Services;

public interface IMemberService
{
    Task<MemberResult> CreateMemberAsync(RegisterMemberRequest request, CancellationToken ct = default);
    Task<MemberResult> DeleteMemberAsync(string id, CancellationToken ct = default);
    Task<MemberResult> UpdateMemberAsync(UpdateMemberRequest request, CancellationToken ct = default);
    Task<MemberDetailsResult> GetMemberDetailAsync(string id, CancellationToken ct = default);
}
