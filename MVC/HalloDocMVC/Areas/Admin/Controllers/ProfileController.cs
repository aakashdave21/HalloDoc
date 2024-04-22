using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HalloDocService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocMVC.Services;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProfileController : Controller
{

    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    

    public IActionResult Index(){
        try
        {
            var AspUserId = User.FindFirstValue("AspUserId");
            AdminProfileViewModel adminProfileView  = _profileService.GetAdminData(int.Parse(AspUserId));
            return View(adminProfileView);
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return View();
        }
    }

    [HttpPost]
    public IActionResult EditAdminInformation(AdminProfileViewModel adminView){
        try
        {
            _profileService.UpdateAdminInfo(adminView);
            TempData["success"] = "Admin Information Updated Successfully!";    
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult EditBillingInformation(AdminProfileViewModel adminView){
        try
        {
            _profileService.UpdateBillingInfo(adminView);
            TempData["success"] = "Admin Information Updated Successfully!";    
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult ResetPassword(AdminProfileViewModel adminView){
        try
        {
            string userPassword = _profileService.GetPassword(adminView.Id);
            // bool isVerified = PasswordHasher.VerifyPassword(adminView.Password, userPassword);
            bool isVerified = userPassword == adminView.Password;
            if(isVerified){
                TempData["error"] = "Password Already Exits, Enter new Password!";    
                return RedirectToAction("Index");
            }

            string hashedPassword = PasswordHasher.HashPassword(adminView.Password);
            // _profileService.UpdatePassword(adminView.Id, adminView.Password);
            _profileService.UpdatePassword(adminView.Id, hashedPassword);
            TempData["success"] = "Password Updated Successfully!";    
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}