using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class RecordsController : Controller
{

    private readonly IRecordsService _recordsService;
    public RecordsController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }
    public IActionResult Index(){
        return View();
    }
    public IActionResult PatientHistory(PatientHistoryView Parameters,int PageNum = 1,int PageSize = 5){
        try
        {   
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest"){
                return PartialView("_PatientHistoryTable", _recordsService.GetPatientHistory(Parameters,PageNum,PageSize));
            }
            return View(_recordsService.GetPatientHistory(Parameters,PageNum,PageSize));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult ExplorePatient(int UserId,int RequestId){
        try
        {   
           return View(_recordsService.GetPatientRequest(UserId,RequestId));
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}