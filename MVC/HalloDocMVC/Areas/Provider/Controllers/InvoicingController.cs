using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Provider.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class InvoicingController : Controller
{
    private readonly IProviderInvoicingService _providerInvoicingService;
    public InvoicingController(IProviderInvoicingService providerInvoicingService)
    {
        _providerInvoicingService = providerInvoicingService;
    }

    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            return View();
        }
    }
    public IActionResult Timesheet(string StartDate, string EndDate)
    {
        try
        {
            return View(_providerInvoicingService.GetTimeSheetList(StartDate, EndDate));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TimeSheetDetailsUpdate(List<TimeSheetDetailsView> timesheetDetailsList,int TimeSheetId)
    {
        try
        {
            var userIdClaim = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int PhysicianId))
            {
                TimeSheetViewModel timeSheet = new(){
                    Id = TimeSheetId,
                    Physicianid = PhysicianId,
                    Startdate = timesheetDetailsList.First().Shiftdate,
                    Enddate = timesheetDetailsList.Last().Shiftdate,
                    Isfinalized = false,
                    TimesheetdetailsList = timesheetDetailsList
                };
                _providerInvoicingService.AddUpdateTimeSheet(timeSheet);
            }
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult CheckForFinalize(string StartDate, string EndDate)
    {
        try
        {
            return Ok(_providerInvoicingService.CheckFinalizeAndGetData(StartDate, EndDate));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}