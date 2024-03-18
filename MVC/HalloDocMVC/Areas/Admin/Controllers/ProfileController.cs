using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HalloDocService.ViewModels;
using HalloDocService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
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
            Console.WriteLine(adminView.FirstName + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.Id + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.AdminId + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.LastName + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.Email + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.ConfirmEmail + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(adminView.Mobile + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            // foreach (var item in adminView.UnCheckedRegions)
            // {
            //     Console.WriteLine(item + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            // }

            _profileService.UpdateAdminInfo(adminView);
            TempData["success"] = "Admin Information Updated Successfully!";    
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}