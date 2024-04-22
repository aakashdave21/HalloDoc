using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class MenuItemsActionFilter : IAsyncActionFilter
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IAccessService _accessService;

    public MenuItemsActionFilter(IAdminDashboardService adminDashboardService,IAccessService accessService)
    {
        _adminDashboardService = adminDashboardService;
        _accessService = accessService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        string? areaName = context.RouteData.Values["area"]?.ToString();
        string? controllerName = context.RouteData.Values["controller"]?.ToString();
        string? actionName = context.RouteData.Values["action"]?.ToString();
        IEnumerable<HeaderMenu> AllMenuList = _accessService.GetAllMenuList();
        if (httpContext.User.Identity.IsAuthenticated && (areaName?.ToLower() == "admin"))
        {
            if (httpContext.User.HasClaim(c => c.Type == "AspUserId"))
            {
                int aspUserId = int.Parse(httpContext.User.FindFirstValue("AspUserId"));
                IEnumerable<HeaderMenu>? menuItems = _adminDashboardService.GetRoleOfUser(aspUserId);
                httpContext.Items["MenuItems"] = menuItems;
                if(areaName==null){
                    await next();
                    return;
                }
                if (!MenuItemsContainItem(AllMenuList, menuItems, areaName, controllerName, actionName))
                {
                    context.Result = new RedirectResult("/Account/AccessDenied");
                    return;
                }
            }
        }
        await next();
    }

    private static bool MenuItemsContainItem(IEnumerable<HeaderMenu> AllMenuList,IEnumerable<HeaderMenu> menuItems, string areaName, string controllerName, string actionName)
    {
        int? AccountType = areaName.ToLower() == "admin" ? 1 : areaName.ToLower() == "provider" ? 2 : 3;
        bool isInAllMenuList = AllMenuList.Any(menu => menu.AccountType == AccountType && menu.Name == controllerName);
        bool isInAssignedMenuList = menuItems.Any(item =>
            item.AccountType == AccountType &&
            item.Name == controllerName);
        if(isInAllMenuList==true && isInAssignedMenuList==false){
            return false;
        }
        return true;
    }
}