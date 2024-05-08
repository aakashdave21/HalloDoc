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
    public InvoicingController(IProviderService providerService,IInvoicingService invoicingService,IProviderInvoicingService providerInvoicingService){
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
            return RedirectToAction("Index","Dashboard");
        }
    }

    public IActionResult GetRecords(int Physician,string StartDate,string EndDate){
        try
        {  
            return Ok(_providerInvoicingService.CheckFinalizeAndGetData(StartDate, EndDate,Physician));
        }
        catch (Exception error)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest(new {Message = error});
        }
    }

    [HttpPost]
    public IActionResult TimeSheet(int Id){
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
}