namespace Domain.Entities;

public class FitnessClass
{
    public string Id { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? InstructorName { get; private set; }
    public string? Category { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    private FitnessClass() { }

    private FitnessClass(
        string id,
        string title,
        string? description,
        string? instructorName,
        string? category,
        DateTime startTime,
        DateTime endTime)
    {
        Id = id;
        Title = title;
        Description = description;
        InstructorName = instructorName;
        Category = category;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static FitnessClass Create(
        string title,
        string? description,
        string? instructorName,
        string? category,
        DateTime startTime,
        DateTime endTime)
        => new(
            Guid.NewGuid().ToString(),
            title,
            description,
            instructorName,
            category,
            startTime,
            endTime
        );

    public void Update(
        string title,
        string? description,
        string? instructorName,
        string? category,
        DateTime startTime,
        DateTime endTime)
    {
        Title = title;
        Description = description;
        InstructorName = instructorName;
        Category = category;
        StartTime = startTime;
        EndTime = endTime;
    }
}