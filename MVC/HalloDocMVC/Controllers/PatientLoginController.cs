using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocMVC.Data;
using HalloDocMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HalloDocMVC.Controllers;

public class PatientLoginController : Controller
{
    private readonly HalloDocContext _context; 

        public PatientLoginController(HalloDocContext context)
        {
            _context = context;
        }


        public IActionResult Index(){
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

        var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == user.Email);
        
        if (userEmail != null)
        {   
            string storedHashPassword = userEmail.Passwordhash;
            // var isPasswordCorrect = PasswordHasher.VerifyPassword(user.Passwordhash , storedHashPassword);  <<<<<<< For Hashing

            var isPasswordCorrect = user.Passwordhash == storedHashPassword ? true : false;

            if(isPasswordCorrect){
                TempData["success"] = "Logged In SuccessFully";
                return RedirectToAction(nameof(ForgotPassword));
            }else{
                TempData["error"] = "Logged In Failed";
                return View(nameof(Index), user);
            }
        }else{
            TempData["error"] = "Logged In Failed";
        }

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
        

        var userDetails = await _context.Aspnetusers.FirstOrDefaultAsync(q=> q.Username == user.Username);
        
        if(userDetails!=null){
            // Send the create Account Link to User Via Email ********
            TempData["success"] = "Reset Link Sent to User Via Email";
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

