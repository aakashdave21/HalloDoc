using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;
using HalloDocRepository.DataModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Interfaces;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using HalloDocService.Provider.Interfaces;
using HalloDocMVC.Services;

namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class ProfileController : Controller
{

    private readonly IProviderService _providerService;
    
    public ProfileController(IProviderService providerService)
    {
        _providerService = providerService;
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
}