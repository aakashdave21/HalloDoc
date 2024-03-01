using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;
using HalloDocRepository.DataModels;
using System.Text.Json.Nodes;
using Org.BouncyCastle.Ocsp;

namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IAdminDashboardService _adminDashboardService;

    public DashboardController(ILogger<DashboardController> logger, IAdminDashboardService adminDashboardService)
    {
        _logger = logger;
        _adminDashboardService = adminDashboardService;
    }


    public async Task<IActionResult> Index(string status)
    {
        (List<RequestViewModel> req, int totalCount) myresult;
        Console.WriteLine(status+"<<<<<<<<<<<<<<<<This is Status");
        ViewBag.statusType = string.IsNullOrEmpty(status) ? "new" : status;
               Console.WriteLine(ViewBag.statusType +"<<<<<<<<<<<<<<<<This is Status");

        var viewModel = new AdminDashboardViewModel();
        var countDictionary = _adminDashboardService.CountRequestByType();

        viewModel.RegionList = await _adminDashboardService.GetRegions();

        viewModel.NewState = countDictionary["new"];
        viewModel.PendingState = countDictionary["pending"];
        viewModel.ActiveState = countDictionary["active"];
        viewModel.ConcludeState = countDictionary["conclude"];
        viewModel.ToCloseState = countDictionary["close"];
        viewModel.UnPaidState = countDictionary["unpaid"];

        string searchBy = Request.Query["searchBy"];
        int pageNumber = Request.Query.TryGetValue("pageNumber", out var pageNumberValue) ? int.Parse(pageNumberValue) : 1;
        int pageSize = Request.Query.TryGetValue("pageSize", out var pageSizeValue) ? int.Parse(pageSizeValue) : 5;
    
        int reqType = 0;
        ViewBag.currentPage = pageNumber;
        ViewBag.currentPageSize = pageSize;

        int startIndex = (pageNumber - 1) * pageSize + 1;

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
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "pending":
                myresult = _adminDashboardService.GetPendingStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "active":
                myresult = _adminDashboardService.GetActiveStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "conclude":
                myresult = _adminDashboardService.GetConcludeStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "close":
                myresult = _adminDashboardService.GetCloseStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "unpaid":
                myresult = _adminDashboardService.GetUnpaidStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            default:
                myresult = _adminDashboardService.GetNewStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
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
public async Task<IActionResult> GetCaseTag(){
    try
    {
        var casetags = await _adminDashboardService.GetCaseTag();
       return Ok(casetags);
    }
    catch (Exception ex)
    {
        _logger.LogInformation(ex.Message);
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}
[HttpPost]
public async Task<IActionResult> CancleCase(IFormCollection formData)
{
    try
    {   
        var reqId = formData["reqId"];
        var reason = formData["reason"];
        var additionalNotes = formData["additionalNotes"];
        _adminDashboardService.CancleRequestCase(int.Parse(reqId),reason,additionalNotes);
        TempData["success"] = "Request Cancelled Successfully!";
       return Json(new { success = true, message = "Form data received successfully" });
    }
    catch (Exception e)
    {
        _logger.LogInformation(e.Message);
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}

public async Task<IActionResult> GetRegions(){
    try
    {
        var regions = await _adminDashboardService.GetRegions();
        return Ok(regions);
    }
    catch (Exception ex)
    {
        _logger.LogInformation(ex.Message);
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}

public async Task<IActionResult> GetPhysicians(int RegionId){
    try
    {   
        var physicians = await _adminDashboardService.GetPhysicianByRegion(RegionId);
        if (physicians == null)
        {
            return Ok(new List<Physician>());
        }
        return Ok(physicians);
    }
    catch (Exception e)
    {
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}

[HttpPost]
public async Task<IActionResult> AssignCase(IFormCollection formData)
{
    try
    {   
        string? Description = formData["description"];
        string? PhysicianId = formData["physician"];
        string? ReqId = formData["reqId"];
        int? AdminId = null;

        await _adminDashboardService.AssignRequestCase(int.Parse(ReqId),int.Parse(PhysicianId),AdminId,Description);

        TempData["success"] = "Request Assigned Successfully!";
       return Json(new { success = true, message = "Form data received successfully" });
    }
    catch (Exception e)
    {
        _logger.LogInformation(e.Message);
        TempData["error"] = "Internal Server Error";
        return Redirect("/Admin/Dashboard/Index");
    }
}

[HttpPost]
public async Task<IActionResult> BlockCase(IFormCollection formData)
{
    try
    {   
        string? Reason = formData["reason"];
        string? ReqId = formData["reqId"];
        int? AdminId = null;

        await _adminDashboardService.BlockRequestCase(int.Parse(ReqId),AdminId,Reason);

        TempData["success"] = "Request Blocked Successfully!";
       return Json(new { success = true, message = "Form data received successfully" });
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
