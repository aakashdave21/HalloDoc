using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HalloDocService.ViewModels;
using HalloDocService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocMVC.Services;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProviderController : Controller
{

    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IProviderService _providerService;
    private readonly IUtilityService _utilityService;


    public ProviderController(IAdminDashboardService adminDashboardService, IProviderService providerService,IUtilityService utilityService)
    {
        _adminDashboardService = adminDashboardService;
        _providerService = providerService;
        _utilityService = utilityService;
    }
    public async Task<IActionResult> Index(string regionId, string order)
    {
        try
        {

            AdminProviderViewModel providerViewModel = _providerService.GetAllProviderData(regionId, order);
            if (regionId != null && order != null)
            {
                ViewBag.Order = order;
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            else if (regionId != null)
            {
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            else if (order == "desc" || order == "asc")
            {
                ViewBag.Order = order;
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            return View(providerViewModel);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index", "Dashboard");
        }
    }
    [HttpPost]
    public IActionResult NotificationChange(AdminProviderViewModel viewModel)
    {
        try
        {
            List<string> stopNotificationIds = viewModel.StopNotificationIds;
            List<string> startNotificationIds = viewModel.StartNotificationIds;
            _providerService.UpdateNotification(stopNotificationIds, startNotificationIds);
            TempData["success"] = "Changes Are Saved Successfully";
            return Ok(new { message = "Updated Successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPost]
    public IActionResult ContactProvider(string Id,string Communication,string Message){
        try
        {
            var PhysicianData = _providerService.GetSingleProviderData(int.Parse(Id));
            _utilityService.EmailSend(null,"aakashdave21@gmail.com",Message,"Message From Admin");
            TempData["success"] = "Message Sent To Physician";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }

}