using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;

namespace HalloDocMVC.Controllers.Admin;

public class AdminDashboardController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAdminDashboardService _adminDashboardService;

    public AdminDashboardController(ILogger<HomeController> logger, IAdminDashboardService adminDashboardService)
    {
        _logger = logger;
        _adminDashboardService = adminDashboardService;
    }

    public IActionResult Index(string status)
    {
        ViewBag.statusType = status ?? "";
        var viewModel = new AdminDashboardViewModel();
        var countDictionary = _adminDashboardService.CountRequestByType();

        viewModel.NewState = countDictionary["new"];
        viewModel.PendingState = countDictionary["pending"];
        viewModel.ActiveState = countDictionary["active"];
        viewModel.ConcludeState = countDictionary["conclude"];
        viewModel.ToCloseState = countDictionary["close"];
        viewModel.UnPaidState = countDictionary["unpaid"];


        if (status == null || status == "new")
        {
            viewModel.Requests = _adminDashboardService.GetNewStatusRequest();
        }
        else if (status == "pending")
        {
            viewModel.Requests = _adminDashboardService.GetPendingStatusRequest();
        }
        else if (status == "active")
        {
            viewModel.Requests = _adminDashboardService.GetActiveStatusRequest();
        }
        else if (status == "conclude")
        {
            viewModel.Requests = _adminDashboardService.GetConcludeStatusRequest();
        }
        else if (status == "close")
        {
            viewModel.Requests = _adminDashboardService.GetCloseStatusRequest();
        }
        else if (status == "unpaid")
        {
            viewModel.Requests = _adminDashboardService.GetUnpaidStatusRequest();
        }

        // Check if the request is made via AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_AdminDashboardTable", viewModel);
        }
        else
        {

            return View("~/Views/Admin/Index.cshtml", viewModel);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
