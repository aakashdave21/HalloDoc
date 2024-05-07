using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Provider.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
using Microsoft.Win32;
namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class InvoicingController : Controller
{
    private readonly IProviderInvoicingService _providerInvoicingService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public InvoicingController(IProviderInvoicingService providerInvoicingService, IWebHostEnvironment hostingEnvironment)
    {
        _providerInvoicingService = providerInvoicingService;
        _hostingEnvironment = hostingEnvironment;
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
            var physicianid = User.FindFirstValue("UserId");
            return View(_providerInvoicingService.GetTimeSheetList(StartDate, EndDate, int.Parse(physicianid)));
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
            var userIdClaim = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int PhysicianId))
            {
                TimeSheetViewModel timeSheet = new()
                {
                    Id = TimeSheetId,
                    Physicianid = PhysicianId,
                    Startdate = timesheetDetailsList.First().Shiftdate,
                    Enddate = timesheetDetailsList.Last().Shiftdate,
                    Isfinalized = false,
                    TimesheetdetailsList = timesheetDetailsList
                };
                _providerInvoicingService.AddUpdateTimeSheet(timeSheet);
            }
            TempData["success"] = "TimeSheet Updated Successfully";

            return RedirectToAction("Timesheet", new { startdate = timesheetDetailsList?.First()?.Shiftdate?.ToString("dd/MM/yyyy"), endDate = timesheetDetailsList?.Last()?.Shiftdate?.ToString("dd/MM/yyyy") });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Timesheet", new { startdate = timesheetDetailsList?.First()?.Shiftdate?.ToString("dd/MM/yyyy"), endDate = timesheetDetailsList?.Last()?.Shiftdate?.ToString("dd/MM/yyyy") });
        }
    }

    [HttpGet]
    public IActionResult CheckForFinalize(string StartDate, string EndDate)
    {
        try
        {
            var physicianid = User.FindFirstValue("UserId");
            return Ok(_providerInvoicingService.CheckFinalizeAndGetData(StartDate, EndDate,int.Parse(physicianid)));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Finalize(int Id)
    {
        try
        {
            _providerInvoicingService.Finalize(Id);
            TempData["success"] = "Timesheet Finialized Successfully";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    public string SaveDocFile(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0) return null;
        string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }
        return filePath;
    }

    [HttpPost]
    public IActionResult SubmitReimbursement(TimesheetreimbursementView timesheetreimbursementView, IFormCollection RestData, IFormFile phyBill)
    {
        try
        {
            DateTime StartDate = DateTime.Parse(RestData["startDate"]);
            DateTime EndDate = DateTime.Parse(RestData["endDate"]);
            var userIdClaim = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int PhysicianId))
            {
                TimeSheetViewModel timeSheet = new()
                {
                    Id = !string.IsNullOrEmpty(RestData["TimeSheetId"]) ? int.Parse(RestData["TimeSheetId"]) : 0,
                    Physicianid = PhysicianId,
                    Startdate = StartDate,
                    Enddate = EndDate,
                    Isfinalized = false,
                };
                timesheetreimbursementView.Bill = SaveDocFile(phyBill, "uploads");
                _providerInvoicingService.AddUpdateTimeReimbursement(timeSheet, timesheetreimbursementView);
            }
            TempData["success"] = "Records Updated Successfully";
            return RedirectToAction("Timesheet", new { startdate = StartDate.ToString("dd/MM/yyyy"), endDate = EndDate.ToString("dd/MM/yyyy") });
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteTimeReimbursement(int Id, string startDate, string endDate)
    {
        DateTime StartDate = DateTime.Parse(startDate);
        DateTime EndDate = DateTime.Parse(endDate);
        try
        {
            _providerInvoicingService.DeleteTimeReimbursement(Id);
            TempData["success"] = "Record Deleted Successfully!";
            return RedirectToAction("Timesheet", new { startdate = StartDate.ToString("dd/MM/yyyy"), endDate = EndDate.ToString("dd/MM/yyyy") });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Timesheet", new { startdate = StartDate.ToString("dd/MM/yyyy"), endDate = EndDate.ToString("dd/MM/yyyy") });
        }
    }


}