using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
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

    public IActionResult Index(){
        try
        {
            return View(_providerService.GetAllProviderLocation());
        }
        catch (System.Exception e)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index","Dashboard");
        }
    }
}