namespace Application.Dtos.Memberships.Requests;

public sealed record CreateMembershipRequest(
    string UserId,
    string MembershipName
);