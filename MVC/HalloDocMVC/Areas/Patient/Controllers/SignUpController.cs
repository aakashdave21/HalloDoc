using Microsoft.AspNetCore.Mvc;
using HalloDocService.ViewModels;
using HalloDocService.Interfaces;

namespace HalloDocMVC.Controllers;

[Area("Patient")]
public class SignUpController : Controller
{

    private readonly IPatientLogin _patientLogin;
    public SignUpController(IPatientLogin patientLogin)
    {
        _patientLogin = patientLogin;
    }

    public IActionResult Index(int userId, string token){
        try
        {
            if(userId != null){
                string UserEmail = _patientLogin.GetAspUserEmail(userId);
                UserLoginViewModel SignUpModel = new(){
                    Email = UserEmail
                };
                return View(SignUpModel);
            }
            return Redirect("/patient/signup?userId="+userId+"&token="+token);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return Redirect("/patient/signup?userId="+userId+"&token="+token);
        }
        
    }

    [HttpPost]
    public IActionResult Index(UserLoginViewModel userModel,string UserId,string UserToken){
         if (!ModelState.IsValid)
        {
            return View(); // Return the view with validation errors
        }
        try
        {
            var tokenDetails = _patientLogin.GetResetTokenExpiry(int.Parse(UserId),UserToken);
            if(tokenDetails.AcivationToken == UserToken && tokenDetails != null && tokenDetails.ActivationExpiry > DateTime.UtcNow ){
                string hashedPassword = Services.PasswordHasher.HashPassword(userModel.Passwordhash);
                _patientLogin.UpdatePassword(tokenDetails.Id,hashedPassword);
                TempData["success"] = "Account Created Successfully!";
                return RedirectToAction("Index","PatientLogin");
            }else if(tokenDetails.ActivationExpiry < DateTime.UtcNow){
                 TempData["error"] = "Oops! Token Expires, Please Go To Login Page";
                 return Redirect("/patient/signup?userId="+UserId+"&token="+UserToken);
            }else if(tokenDetails.AcivationToken != UserToken){
                TempData["error"] = "Oops! Token Expires or Incorrect, Please Go To Login Page";
               return Redirect("/patient/signup?userId="+UserId+"&token="+UserToken);
            }
            TempData["error"] = "Internal Server Error";
            return Redirect("/patient/signup?userId="+UserId+"&token="+UserToken);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return Redirect("/patient/signup?userId="+UserId+"&token="+UserToken);
        }
       
    }
}