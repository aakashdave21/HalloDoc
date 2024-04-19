using Microsoft.AspNetCore.Mvc;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Interfaces;
using HalloDocService.Provider.Interfaces;
using HalloDocMVC.Services;

namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class ProfileController : Controller
{

    private readonly IProviderService _providerService;
    private readonly IProviderDashboardService _providerDashboardService;
    private readonly IUtilityService _utilityService;
    
    public ProfileController(IProviderService providerService,IProviderDashboardService providerDashboardService,IUtilityService utilityService)
    {
        _providerService = providerService;
        _providerDashboardService = providerDashboardService;
        _utilityService = utilityService;
    }
    public IActionResult Index(){
         try
        {
            string? Id = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(Id))
            {
                AdminPhysicianEditViewModel adminViewModel = _providerService.GetPhyisicianData(int.Parse(Id));
                if (!string.IsNullOrEmpty(adminViewModel.UploadSign))
                {
                    if (Path.IsPathRooted(adminViewModel.UploadSign))
                    {
                        adminViewModel.UploadSign = Path.Combine("uploads", Path.GetFileName(adminViewModel.UploadSign));
                    }
                }
                adminViewModel.UploadPhoto = !string.IsNullOrEmpty(adminViewModel.UploadPhoto) ?
                            Path.Combine("uploads", Path.GetFileName(adminViewModel.UploadPhoto)) :
                            adminViewModel.UploadPhoto;
                return View(adminViewModel);
            }
            return RedirectToAction("/Account/NotFound");
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult SetPassword(string password, string Id)
    {
        try
        {
            string hashedPassword = PasswordHasher.HashPassword(password);
            _providerService.UpdateProviderPassword(int.Parse(Id), hashedPassword);
            return Ok(new { message = "Successfully Reset Password" });
        }
        catch (System.Exception e)
        {
            return BadRequest(new { message = e });
        }
    }

    [HttpPost]
    public IActionResult SendRequest(string RequestNote){
        try
        {
            int? PhyId = int.Parse(User.FindFirstValue("UserId"));
            var AdminMail = _providerDashboardService.SendProfileRequest(PhyId);
            if(!string.IsNullOrEmpty(AdminMail.Email)){
                _utilityService.EmailSend("aakashdave21@gmail.com" , RequestNote , "Request For Changing Information", null , 1 , null , null , AdminMail.AdminAspnetusers.FirstOrDefault()?.Id);
                TempData["success"] = "Request Sent To Admin!";
            }else{
                TempData["error"] = "There Are No Admin Assign To You!";
            }
            return RedirectToAction("Index");
        }
        catch (System.Exception e)
        {
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }
}