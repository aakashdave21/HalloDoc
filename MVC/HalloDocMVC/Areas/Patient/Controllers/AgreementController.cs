using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.Interfaces;
using HalloDocService.Admin.Interfaces;

namespace HalloDocMVC.Controllers;

[Area("Patient")]
public class AgreementController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;

    public AgreementController(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    public IActionResult Index(string reqId, string token)
    {
        var tokenDetails = _adminDashboardService.GetSingleRequest(int.Parse(reqId));
        if(reqId!=null && token!=null && tokenDetails.Accepteddate==null && tokenDetails.AcceptToken!=null && tokenDetails.AcceptToken == token && tokenDetails.AcceptExpiry > DateTime.UtcNow && tokenDetails.Status == 2){
            return View();
        }
        TempData["error"] = "Something went wrong !";
        return NotFound();
    }

    public IActionResult AgreementSubmit(string RequestId, string RequestToken){
        try
        {
            var tokenDetails = _adminDashboardService.GetSingleRequest(int.Parse(RequestId));
            if(tokenDetails != null && tokenDetails.AcceptToken == RequestToken && tokenDetails.AcceptExpiry > DateTime.UtcNow){
                _adminDashboardService.AgreementAccept(tokenDetails.Id);
                TempData["success"] = "Agreement Accepted!";
                return RedirectToAction(nameof(Index),"Home");
            }else if(tokenDetails.AcceptExpiry < DateTime.UtcNow){

                // Update Request to Cancel
                TempData["error"] = "Oops! Time has been Ended! Please Create New Request";
                return RedirectToAction(nameof(Index));
            }else if(tokenDetails.AcceptToken != RequestToken){
                TempData["error"] = "Oops! Incorrect Details! Please Visit Link Again";
            }
            throw new Exception();
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index",new { reqId = RequestId, token = RequestToken });
        }
    }
    public IActionResult AgreementCancel(string reason,string RequestId, string RequestToken){
        try
        {
            var tokenDetails = _adminDashboardService.GetSingleRequest(int.Parse(RequestId));
            if(tokenDetails != null && tokenDetails.AcceptToken == RequestToken && tokenDetails.AcceptExpiry > DateTime.UtcNow){
                _adminDashboardService.AgreementReject(tokenDetails.Id,reason);
                TempData["success"] = "Agreement Cancelled!";
                return RedirectToAction(nameof(Index),"Home");
            }else if(tokenDetails.AcceptExpiry < DateTime.UtcNow){

                // Update Request to Cancel
                TempData["error"] = "Oops! Time has been Ended! Please Create New Request";
                return RedirectToAction(nameof(Index));
            }else if(tokenDetails.AcceptToken != RequestToken){
                TempData["error"] = "Oops! Incorrect Details! Please Visit Link Again";
            }
            throw new Exception();
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index",new { reqId = RequestId, token = RequestToken });
        }
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
