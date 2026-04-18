using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.MenuNavigation;

namespace Presentation.WebApp.Services.MenuNavigation;

public sealed class MenuNavigationService(IActionDescriptorCollectionProvider actionProvider, LinkGenerator linkGenerator) : IMenuNavigationService
{
    public IReadOnlyList<MenuNavigationItem> GetMenu()
    {
        var actions = actionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>().Where(IsVisibleInMenu).Select(action => new MenuNavigationActionItem
        {
            ControllerName = action.ControllerName,
            ActionName = action.ActionName,
            DisplayName = GetDisplayName(action),
            Order = GetOrder(action),
            Url = linkGenerator.GetPathByAction(action: action.ActionName, controller: action.ControllerName)
        }).Where(x => !string.IsNullOrWhiteSpace(x.Url)).GroupBy(x => x.ControllerName).OrderBy(g => g.Min(x => x.Order)).ThenBy(g => g.Key).Select(group =>
        {
            var children = group.OrderBy(x => x.Order).ThenBy(x => x.DisplayName).Select(x => new MenuNavigationChildItem
            {
                ControllerName = x.ControllerName,
                ActionName = x.ActionName,
                DisplayName = x.DisplayName,
                Url = x.Url!
            }).ToList();
            return new MenuNavigationItem
            {
                ControllerName = group.Key,
                DisplayName = GetControllerDisplayName(group.Key, children),
                Url = children.Count == 1 ? children[0].Url : null,
                Children = children
            };
        }).ToList();

        return actions;
    }

    private static bool IsVisibleInMenu(ControllerActionDescriptor action)
    {
        if (action.MethodInfo.GetCustomAttributes(typeof(NonActionAttribute), inherit: true).Length != 0)
            return false;

        if (action.MethodInfo.GetCustomAttributes(typeof(HideInMenuAttribute), inherit: true).Length != 0)
            return false;

        if (action.MethodInfo.GetParameters().Length > 0)
            return false;

        var httpMethodsAttributes = action.MethodInfo.GetCustomAttributes(inherit: true).OfType<HttpMethodAttribute>().ToList();
    
        if(httpMethodsAttributes.Count != 0)
        {
            var allowedMethods = httpMethodsAttributes.SelectMany(x => x.HttpMethods).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            if (!allowedMethods.Contains("GET", StringComparer.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    private static string GetDisplayName(ControllerActionDescriptor action)
    {
        var menuItemAttribute = action.MethodInfo.GetCustomAttributes(typeof(MenuItemAttribute), inherit: true).OfType<MenuItemAttribute>().FirstOrDefault();

        return menuItemAttribute?.Title ?? action.ActionName;
    }

    private static int GetOrder(ControllerActionDescriptor action)
    {
        var menuItemAttribute = action.MethodInfo.GetCustomAttributes(typeof(MenuItemAttribute), inherit: true).OfType<MenuItemAttribute>().FirstOrDefault();

        return menuItemAttribute?.Order ?? 1000;
    }

    private static string GetControllerDisplayName(string controllerName, List<MenuNavigationChildItem> children)
    {
        if (children.Count == 1)
            return children[0].DisplayName;

        return controllerName;
    }
}
