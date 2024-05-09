using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocService.Provider.Interfaces;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class InvoicingController : Controller
{

    private readonly IProviderService _providerService;
    private readonly IInvoicingService _invoicingService;
    private readonly IProviderInvoicingService _providerInvoicingService;
    public InvoicingController(IProviderService providerService, IInvoicingService invoicingService, IProviderInvoicingService providerInvoicingService)
    {
        _providerService = providerService;
        _invoicingService = invoicingService;
        _providerInvoicingService = providerInvoicingService;
    }

    public IActionResult Index()
    {
        try
        {
            TimeSheetViewModel timeSheetViewModel = new();
            timeSheetViewModel.ProviderLists = _providerService.GetAllPhysicianList();
            return View(timeSheetViewModel);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    public IActionResult GetRecords(int Physician, string StartDate, string EndDate)
    {
        try
        {
            return Ok(_providerInvoicingService.CheckFinalizeAndGetData(StartDate, EndDate, Physician));
        }
        catch (Exception error)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest(new { Message = error });
        }
    }

    [HttpPost]
    [HttpGet]
    public IActionResult TimeSheet(int Id)
    {
        try
        {
            return View(_providerInvoicingService.GetTimeSheetById(Id));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TimeSheetDetailsUpdate(List<TimeSheetDetailsView> timesheetDetailsList, int TimeSheetId)
    {
        try
        {
            TimeSheetViewModel timeSheet = new()
            {
                Id = TimeSheetId,
                Startdate = timesheetDetailsList.First().Shiftdate,
                Enddate = timesheetDetailsList.Last().Shiftdate,
                Isfinalized = false,
                TimesheetdetailsList = timesheetDetailsList
            };
            _providerInvoicingService.AddUpdateTimeSheet(timeSheet);
            TempData["success"] = "TimeSheet Updated Successfully";

            return RedirectToAction("Timesheet", new { id = TimeSheetId});
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Timesheet", new { id = TimeSheetId});
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Approve(int Id, int Bonus, string Description){
        try
        {
            _providerInvoicingService.ApproveTimeSheet(Id, Bonus, Description);
            TempData["success"] = "TimeSheet Approved Successfully";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Timesheet", new { id = Id});
        }
    }
}