namespace Domain.Entities;

public class Membership
{
    public string Id { get; private set; } = null!;
    public string MemberId { get; private set; } = null!;
    public string MembershipName { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private Membership() { }

    private Membership(string id, string memberId, string membershipName, DateTime startDate, DateTime endDate)
    {
        Id = id;
        MemberId = memberId;
        MembershipName = membershipName;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Membership Create(string memberId, string membershipName, DateTime startDate, DateTime endDate)
        => new(Guid.NewGuid().ToString(), memberId, membershipName, startDate, endDate);
}