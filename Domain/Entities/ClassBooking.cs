namespace Domain.Entities;

public class ClassBooking
{
    public string Id { get; private set; } = null!;
    public string MemberId { get; private set; } = null!;
    public string FitnessClassId { get; private set; } = null!;
    public DateTime BookedAt { get; private set; }

    private ClassBooking() { }

    private ClassBooking(string id, string memberId, string fitnessClassId, DateTime bookedAt)
    {
        Id = id;
        MemberId = memberId;
        FitnessClassId = fitnessClassId;
        BookedAt = bookedAt;
    }

    public static ClassBooking Create(string memberId, string fitnessClassId)
        => new(Guid.NewGuid().ToString(), memberId, fitnessClassId, DateTime.UtcNow);
}