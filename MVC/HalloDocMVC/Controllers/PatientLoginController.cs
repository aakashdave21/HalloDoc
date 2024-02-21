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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace HalloDocMVC.Controllers;

public class PatientLoginController : Controller
{
    private readonly IPatientLogin _patientLoginService;



    public PatientLoginController(IPatientLogin patientLoginService)
    {
        _patientLoginService = patientLoginService;
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

                string senderEmail = "tatva.dotnet.aakashdave@outlook.com";
                // string senderEmail = "aakashdave21@gmail.com";
                string senderPassword = "Aakash21##";
                // string senderPassword = "buicbfrijgnvrttn";

                SmtpClient client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "HalloDoc"),
                    Subject = "Set up your Account",
                    IsBodyHtml = true,
                    Body = $"Please click the following link to reset your password: <a href='{callbackUrl}'>{callbackUrl}</a>"
                };

                mailMessage.To.Add("aakashdave21@gmail.com");

                client.Send(mailMessage);
                // string fromMail = "testkirtan04@gmail.com";
                // // string fromMail = "test.dotnet@etatvasoft.com";
                // // string fromMail = "project.homebuddy.01@gmail.com";
                // // string fromMail = "architsolnki@gmail.com";
                // // string fromPassword = "zdjm lbja mwgi zyou";
                // string fromPassword = "cihv cpfv toya yjfu";
                // // string fromPassword = "P}N^{z-]7Ilp";
                // // string fromPassword = "xjoytqbqgfwyqwim";
                // // string fromPassword = "cawb cjlh tftj tuve";

                // MailMessage msg = new MailMessage();
                // msg.From = new MailAddress(fromMail);
                // msg.Subject = "Test";
                // msg.To.Add(new MailAddress("aakashdave21@gmail.com"));
                // msg.Body = "Test";
                // msg.IsBodyHtml = true;

                // var smtpClient = new SmtpClient("smtp.gmail.com"){
                // // var smtpClient = new SmtpClient("mail.etatvasoft.com"){
                //     Port = 587,
                //     Credentials = new NetworkCredential(fromMail,fromPassword),
                //     EnableSsl = true,

                // };

                // smtpClient.Send(msg);

                // var callbackUrl = Url.Action("Index", "Forgot", new { userId = user.Id, token }, protocol: HttpContext.Request.Scheme);

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
    public async Task<IActionResult> ResetPasswordPost(UserResetPasswordViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return View("ResetPassword"); // Return the view with validation errors
        }
        try
        {
            var token = _patientLoginService.GetResetTokenExpiry(user.UserId, user.UserToken);
            if (token.ResetToken == user.UserToken && token != null && token.ResetExpiration > DateTime.UtcNow)
            {
                TempData["success"] = "Password Reset Successfully !";
                return RedirectToAction(nameof(Index));
            }
            else if (token.ResetExpiration < DateTime.UtcNow)
            {
                TempData["error"] = "Oops! Token Expires, Please Go To Login Page";
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

public class MailService
{
    private const string SenderEmail = "aakashdave21@gmail.com"; // Your Gmail email address
    private const string SenderPassword = "buicbfrijgnvrttn"; // Your application-specific password

    private readonly SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", 587);

    public MailService()
    {
        _smtpClient.EnableSsl = true;
        _smtpClient.UseDefaultCredentials = false;
        _smtpClient.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
    }

    public async Task<bool> SendAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage(SenderEmail, recipientEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(mailMessage);

            // Email sent successfully, return true
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"Failed to send email: {ex.Message}");
            Console.WriteLine(ex);

            // Return false to indicate that the email sending failed
            return false;
        }
    }
}