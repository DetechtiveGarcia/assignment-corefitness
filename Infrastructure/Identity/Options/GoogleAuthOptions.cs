namespace Infrastructure.Identity.Options;

public sealed class GoogleAuthOptions
{
    public const string SectionName = "Authentication:Google";

    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
}