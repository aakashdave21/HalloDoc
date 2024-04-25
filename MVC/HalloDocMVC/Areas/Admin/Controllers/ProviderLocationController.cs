using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProviderLocationController : Controller
{


    private readonly IProviderService _providerService;

    public ProviderLocationController(IProviderService providerService)
    {
        _providerService = providerService;
    }

    public IActionResult Index()
    {
        try
        {
            return View(_providerService.GetAllProviderLocation());
        }
        catch (Exception e)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}