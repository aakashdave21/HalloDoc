using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SMSLogsController : Controller
{
    private readonly IRecordsService _recordsService;
    public SMSLogsController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }

    public IActionResult Index(EmailLogsView Parameters, int PageNum = 1, int PageSize = 5){
        try
        {
            
            if(Request.Headers["X-Requested-With"]=="XMLHttpRequest"){
                return PartialView("_SMSLogTable",_recordsService.SMSLogs(Parameters, PageNum, PageSize));
            }
            return View(_recordsService.SMSLogs(Parameters, PageNum, PageSize));
        }
        catch (System.Exception e)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index","Dashboard");
        }
    }
}