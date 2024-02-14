using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
// using HalloDocMVC.Data;
// using HalloDocMVC.ViewModels;
using Microsoft.EntityFrameworkCore;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace HalloDocMVC.Controllers;

public class PatientLoginController : Controller
{
        private readonly IPatientLogin _patientLoginService; 

        public PatientLoginController(IPatientLogin patientLoginService)
        {
            _patientLoginService = patientLoginService;
        }


        public IActionResult Index(){

            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index),"Dashboard");
            return View();
        }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost([Bind("Email,Passwordhash")] UserLoginViewModel user)
    {

        if (!ModelState.IsValid)
        {   
            return View(nameof(Index), user); // Return the view with validation errors
        }

        var userEmail =  _patientLoginService.ValidateUser(user);
        
        if (userEmail != null)
        {   
            string storedHashPassword = userEmail.Passwordhash;
            // var isPasswordCorrect = PasswordHasher.VerifyPassword(user.Passwordhash , storedHashPassword);  <<<<<<< For Hashing
            var isPasswordCorrect = _patientLoginService.VerifyPassword(user.Passwordhash,storedHashPassword);

            if(isPasswordCorrect){
                var userDetails = _patientLoginService.UserDetailsFetch(userEmail.Email);
                

                // Authentication Logic Start Here
                List<Claim> claims = new List<Claim>(){
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, userEmail.Username),
                    new Claim("UserId", userDetails.Id.ToString()),
                    new Claim("OtherProperties","Example Role")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                AuthenticationProperties properties = new AuthenticationProperties(){
                    AllowRefresh = true,
                    // IsPersistent 
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),properties);

                TempData["success"] = "Logged In SuccessFully";

                return RedirectToAction(nameof(Index),"Dashboard");
                // return RedirectToAction(nameof(Index));
            }
        }
        TempData["error"] = "Logged In Failed";
        return View(nameof(Index), user);
    }


    // Forgot Password View
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
 
    public async Task<IActionResult> ForgotPasswordPost([Bind("Username")] UserResetPasswordViewModel user){

        if (!ModelState.IsValid)
        {
            return View(nameof(ForgotPassword), user); // Return the view with validation errors
        }
        
        var userDetails = _patientLoginService.FindUserFromUsername(user);
        
        if(userDetails!=null){
            // Send the create Account Link to User Via Email ********
            TempData["success"] = "Reset Link Sent to User Via Email " + userDetails.Email;
            return RedirectToAction(nameof(Index));
        }else{
            TempData["error"] = "User does not exists";
            return RedirectToAction(nameof(ForgotPassword));
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

