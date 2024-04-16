using HalloDocRepository.DataModels;
using HalloDocService.Admin.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

public class MenuItemsActionFilter : IAsyncActionFilter
{
    private readonly IAdminDashboardService _adminDashboardService;

    public MenuItemsActionFilter(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // var httpContext = context.HttpContext;
        // string? areaName = context.RouteData.Values["area"]?.ToString();
        // if (httpContext.User.Identity.IsAuthenticated && areaName?.ToLower() == "admin")
        // {
        //     if (httpContext.User.HasClaim(c => c.Type == "AspUserId"))
        //     {
        //         var aspUserId = int.Parse(httpContext.User.FindFirstValue("AspUserId"));
        //         var menuItems = _adminDashboardService.GetRoleOfUser(aspUserId);
        //         httpContext.Items["MenuItems"] = menuItems;
        //         Console.WriteLine(menuItems + "<<<<<<<<<<<<<<<<<<<<<");
        //         var controllerName = context.RouteData.Values["controller"]?.ToString();
        //         var actionName = context.RouteData.Values["action"]?.ToString();
        //         Console.WriteLine(controllerName);
        //         Console.WriteLine(actionName);
        //         Console.WriteLine(areaName);
        //         if(areaName==null){
        //             await next();
        //             return;
        //         }
        //         if (!MenuItemsContainItem(menuItems, areaName, controllerName, actionName))
        //         {
        //             Console.WriteLine("In If");
        //             context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
        //             return;
        //         }
        //     }
        // }
        await next();
    }

    private static bool MenuItemsContainItem(IEnumerable<HeaderMenu> menuItems, string areaName, string controllerName, string actionName)
    {
        int? AccountType = areaName.ToLower() == "admin" ? 1 : areaName.ToLower() == "provider" ? 2 : 3;
        return menuItems.Any(item =>
            item.AccountType == AccountType &&
            item.Name == controllerName);
    }
}