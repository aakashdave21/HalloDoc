using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocMVC.Data;

namespace HalloDocMVC.Controllers;

public class PatientLoginController : Controller
{
    private readonly ILogger<PatientLoginController> _logger;

    public PatientLoginController(ILogger<PatientLoginController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IndexPost([Bind("Email,Passwordhash")] Aspnetuser user)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), user); // Return the view with validation errors
        }
        if (string.IsNullOrEmpty(user.Email))
        {
            ModelState.AddModelError("Email", "Email is required.");
            return View(nameof(Index), user); // Return the view with error message
        }
        if (string.IsNullOrEmpty(user.Passwordhash))
        {
            ModelState.AddModelError("Passwordhash", "Password is required.");
            return View(nameof(Index), user); // Return the view with error message
        }

        Console.WriteLine(user.Email, "<<<<<< This is User");
        Console.WriteLine(user.Passwordhash, "<<<< Password");

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
