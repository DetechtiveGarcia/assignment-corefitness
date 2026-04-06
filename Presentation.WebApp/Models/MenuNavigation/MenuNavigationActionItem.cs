namespace Presentation.WebApp.Models.MenuNavigation;

public class MenuNavigationActionItem
{
    public required string ControllerName { get; init; }
    public required string ActionName { get; init; }
    public required string DisplayName { get; init; }
    public required int Order { get; init; }
    public string? Url { get; init; }
}
