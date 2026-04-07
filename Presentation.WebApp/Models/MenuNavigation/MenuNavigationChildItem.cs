namespace Presentation.WebApp.Models.MenuNavigation;

public class MenuNavigationChildItem
{
    public required string ControllerName { get; init; }
    public required string ActionName { get; init; }
    public required string DisplayName { get; init; }
    public required string Url { get; init; }
}