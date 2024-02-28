using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;
using HalloDocRepository.DataModels;

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
        (List<RequestViewModel> req, int totalCount) myresult;
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
        // int pageNumber = int.Parse(Request.Query["pageNumber"]);
        // int pageSize = int.Parse(Request.Query["pageSize"]);
        int pageNumber = 1;
        int pageSize = 3;
        int reqType = 0;
        if (!string.IsNullOrEmpty(Request.Query["requesttype"]))
        {
            int.TryParse(Request.Query["requesttype"], out reqType);
        }
        switch (status)
        {
            case "new":
                myresult = _adminDashboardService.GetNewStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
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
                myresult = _adminDashboardService.GetNewStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
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
            if(viewnotes!=null){
                return View("ViewNotes",viewnotes);
            }else{
                return View("ViViewNotesew");
            }
        }
        catch (Exception e)
        {   
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");

        }
    } 

[HttpPost]
public async Task<IActionResult> SaveViewNotes(ViewNotesViewModel viewnotes)
{
    try
    {
        _adminDashboardService.SaveAdditionalNotes(viewnotes?.AdditionalNote,viewnotes.NoteId,viewnotes.ReqId);
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
