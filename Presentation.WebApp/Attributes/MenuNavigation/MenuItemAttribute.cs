namespace Presentation.WebApp.Attributes.MenuNavigation;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MenuItemAttribute(string title, int order = 1000) : Attribute
{
    public string Title { get; } = title;
    public int Order { get; } = order;
}
