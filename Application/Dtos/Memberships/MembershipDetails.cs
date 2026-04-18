namespace Application.Dtos.Memberships;

public record MembershipDetails
(
    string Id,
    string MembershipName,
    DateTime StartDate,
    DateTime EndDate
);
