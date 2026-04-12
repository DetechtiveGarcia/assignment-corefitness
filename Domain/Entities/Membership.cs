namespace Domain.Entities;
public class Membership
{
    public string Id { get; private set; } = null!;
    public string? MemberId { get; private set; }
    public string? MembershipOptionId { get; private set; }
    public DateTimeOffset? StartDate { get; private set; } //DateTime eller DateTimeOffset??
    public DateTime? EndDate { get; private set; }
}
