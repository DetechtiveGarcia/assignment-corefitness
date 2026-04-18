using Presentation.WebApp.Models.MenuNavigation;

namespace Presentation.WebApp.Services.MenuNavigation;

public interface IMenuNavigationService
{
    IReadOnlyList<MenuNavigationItem> GetMenu();
}
