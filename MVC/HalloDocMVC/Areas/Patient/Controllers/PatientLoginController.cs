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


namespace HalloDocMVC.Controllers;

[Area("Patient")]
public class PatientLoginController : Controller
{
    private readonly IPatientLogin _patientLoginService;
    private readonly IUtilityService _utilityService;



    public PatientLoginController(IPatientLogin patientLoginService,IUtilityService utilityService)
    {
        _patientLoginService = patientLoginService;
        _utilityService = utilityService;
    }


    public IActionResult Index()
    {

        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction(nameof(Index), "Dashboard");
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

        var userEmail = _patientLoginService.ValidateUser(user);

        if (userEmail != null)
        {
            string storedHashPassword = userEmail.Passwordhash;
            // var isPasswordCorrect = PasswordHasher.VerifyPassword(user.Passwordhash , storedHashPassword);  <<<<<<< For Hashing
            var isPasswordCorrect = _patientLoginService.VerifyPassword(user.Passwordhash, storedHashPassword);

            if (isPasswordCorrect)
            {
                var userDetails = _patientLoginService.UserDetailsFetch(userEmail.Email);


                // Authentication Logic Start Here
                List<Claim> claims = new List<Claim>(){
                    new Claim(ClaimTypes.Role, "Patient"),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, userEmail.Username),
                    new Claim("UserId", userDetails.Id.ToString()),
                    new Claim("OtherProperties","Example Role")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    // IsPersistent 
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

                TempData["success"] = "Logged In SuccessFully";

                return RedirectToAction(nameof(Index), "Dashboard");
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

    public async Task<IActionResult> ForgotPasswordPost([Bind("Username")] UserResetPasswordViewModel user)
    {

        if (!ModelState.IsValid)
        {
            return View(nameof(ForgotPassword), user); // Return the view with validation errors
        }
        try
        {
            var userDetails = _patientLoginService.FindUserFromUsername(user);
            if (userDetails != null)
            {
                string token = Guid.NewGuid().ToString();
                var callbackUrl = Url.Action("ResetPassword", "PatientLogin", new { userId = userDetails.Id, token }, protocol: HttpContext.Request.Scheme);
                DateTime expirationTime = DateTime.UtcNow.AddHours(1);
                _patientLoginService.StoreResetToken(userDetails.Id, token, expirationTime);

                Console.WriteLine(callbackUrl);

                string rcvrMail = "aakashdave21@gmail.com";
                await _utilityService.EmailSend(callbackUrl,rcvrMail);
                

                TempData["success"] = "Reset Link Sent to User Via Email " + userDetails.Email;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "User does not exists";
                return RedirectToAction(nameof(ForgotPassword));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            TempData["error"] = "Internal Server Error";
            return RedirectToAction(nameof(Index));
        }



    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPasswordPost(ResetPasswordViewModel users)
    {   
        if (!ModelState.IsValid)
        {
            return View("ResetPassword"); // Return the view with validation errors
        }
        try
        {
            
            var token = _patientLoginService.GetResetTokenExpiry(users.UserId, users.UserToken);
            if (token.ResetToken == users.UserToken && token != null && token.ResetExpiration > DateTime.UtcNow)
            {
                _patientLoginService.UpdatePassword(token.Id , users.Password);
                TempData["success"] = "Password Reset Successfully !";
                return RedirectToAction(nameof(Index));
            }
            else if (token.ResetExpiration < DateTime.UtcNow)
            {
                TempData["error"] = "Oops! Token Expires, Please Go To Login Page";
                return RedirectToAction(nameof(Index));
            }else if(token.ResetToken != users.UserToken){
                TempData["error"] = "Oops! Token Expires or Incorrect, Please Go To Login Page";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Internal Server Error";
            return View("ResetPassword");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            TempData["error"] = "Internal Server Error";
            return RedirectToAction(nameof(Index));
        }
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}