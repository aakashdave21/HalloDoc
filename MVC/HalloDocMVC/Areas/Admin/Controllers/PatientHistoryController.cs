using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PatientHistoryController : Controller
{
    private readonly IRecordsService _recordsService;
    public PatientHistoryController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }

    public IActionResult Index(PatientHistoryView Parameters, int PageNum = 1, int PageSize = 5)
    {
        try
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PatientHistoryTable", _recordsService.GetPatientHistory(Parameters, PageNum, PageSize));
            }
            return View(_recordsService.GetPatientHistory(Parameters, PageNum, PageSize));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }
    public IActionResult ExplorePatient(int UserId, int RequestId)
    {
        try
        {
            return View(_recordsService.GetPatientRequest(UserId, RequestId));
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


}