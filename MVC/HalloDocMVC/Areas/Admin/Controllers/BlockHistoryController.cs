using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BlockHistoryController : Controller
{
    private readonly IRecordsService _recordsService;
    public BlockHistoryController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }

    public IActionResult Index(EmailLogsView Parameters, int PageNum = 1, int PageSize = 5){
        try
        {
            if(Request.Headers["X-Requested-With"]=="XMLHttpRequest"){
                return PartialView("_blockHistoryTable",_recordsService.BlockHistory(Parameters, PageNum, PageSize));
            }
            return View(_recordsService.BlockHistory(Parameters, PageNum, PageSize));
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index","Dashboard");
        }
    }

    public IActionResult Unblock(int Id){
        try
        {
            _recordsService.UnblockRequest(Id);
            TempData["Success"] = "Request Unblocked Successfully!";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}