namespace Presentation.WebApp.Models.MenuNavigation;

public class MenuNavigationItem
{
    public required string ControllerName { get; init; }
    public required string DisplayName { get; init; }
    public string? Url { get; init; }
    public List<MenuNavigationChildItem> Children { get; init; } = [];
}
