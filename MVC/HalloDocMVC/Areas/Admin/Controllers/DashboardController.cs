using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;

namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAdminDashboardService _adminDashboardService;

    public DashboardController(ILogger<HomeController> logger, IAdminDashboardService adminDashboardService)
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

        string searchBy = Request.Query["searchBy"];
       int reqType = 0;
        if (!string.IsNullOrEmpty(Request.Query["requesttype"]))
        {
            int.TryParse(Request.Query["requesttype"], out reqType);
        }

        Console.WriteLine(searchBy);
        Console.WriteLine(status);
        Console.WriteLine(reqType+"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

        switch (status)
        {
            case "new":
                viewModel.Requests = _adminDashboardService.GetNewStatusRequest(searchBy,reqType);
                break;
            case "pending":
                viewModel.Requests = _adminDashboardService.GetPendingStatusRequest(searchBy,reqType);
                break;
            case "active":
                viewModel.Requests = _adminDashboardService.GetActiveStatusRequest(searchBy,reqType);
                break;
            case "conclude":
                viewModel.Requests = _adminDashboardService.GetConcludeStatusRequest(searchBy,reqType);
                break;
            case "close":
                viewModel.Requests = _adminDashboardService.GetCloseStatusRequest(searchBy,reqType);
                break;
            case "unpaid":
                viewModel.Requests = _adminDashboardService.GetUnpaidStatusRequest(searchBy,reqType);
                break;
            default:
                viewModel.Requests = _adminDashboardService.GetNewStatusRequest(searchBy,reqType);
                break;
        }

        // Check if the request is made via AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_AdminDashboardTable", viewModel);
        }
        else
        {
            return View(viewModel);
        }
    }

    public async Task<IActionResult> ViewCase(int id)
    {
        try
        {
            ViewCaseViewModel viewcase = _adminDashboardService.GetViewCaseDetails(id);
            Console.WriteLine(viewcase.Phone);
            return View("ViewCase",viewcase);
        }
        catch (Exception e)
        {   
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public async Task<IActionResult> ViewNotes(int id)
    {
        try
        {
            ViewNotesViewModel viewnotes = _adminDashboardService.GetViewNotesDetails(id);
            return View("ViewNotes",viewnotes);
        }
        catch (Exception e)
        {   
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");

        }
    } 

[HttpPost]
public async Task<IActionResult> SaveViewNotes(ViewNotesViewModel viewnotes)
{
    try
    {
        _adminDashboardService.SaveAdditionalNotes(viewnotes.AdditionalNote,viewnotes.NoteId);
         TempData["success"] = "Updated Successfully";
        return Redirect("/admin/dashboard/ViewNotes/"+viewnotes.ReqId);
    }
    catch (Exception e)
    {
        _logger.LogInformation(e.Message);
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
