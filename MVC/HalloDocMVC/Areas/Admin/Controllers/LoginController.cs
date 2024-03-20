using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HalloDocService.ViewModels;
using HalloDocService.Interfaces;
using HalloDocMVC.Services;

namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
public class LoginController : Controller
{

    private readonly IPatientLogin _userLogin;

    public LoginController(IPatientLogin userLogin)
    {
        _userLogin = userLogin;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction(nameof(Index), "Dashboard");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(UserLoginViewModel userView)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), userView);
            }
            Console.WriteLine(userView.Email);
            Console.WriteLine(userView.Passwordhash);

            var userEmail = _userLogin.ValidateUser(userView);
            var roles = userEmail.Aspnetuserroles.ToList();
            bool IsPatient = false;
            bool IsAdmin = false;
            foreach (var item in roles)
            {
                if (string.Equals(item.Role.Name, "patient", StringComparison.OrdinalIgnoreCase))
                {
                    IsPatient = true;
                }
                if (string.Equals(item.Role.Name, "admin", StringComparison.OrdinalIgnoreCase))
                {
                    IsAdmin = true;
                }
            }

            if (userEmail != null)
            {
                string storedHashPassword = userEmail.Passwordhash;
                
                var isPasswordCorrectHashed = PasswordHasher.VerifyPassword(userView.Passwordhash , storedHashPassword);
                // var isPasswordCorrect = _userLogin.VerifyPassword(userView.Passwordhash, storedHashPassword);

                if (isPasswordCorrectHashed && IsAdmin)
                {
                    var userDetails = _userLogin.UserDetailsFetch(userEmail.Email);

                    // Authentication
                    List<Claim> claims = new(){
                        new Claim(ClaimTypes.Role,"Admin"),
                        new Claim(ClaimTypes.NameIdentifier,userView.Email),
                        new Claim(ClaimTypes.Name,userEmail.Username),
                        new Claim("AspUserId",userDetails.Aspnetuser.Id.ToString() ?? ""),
                        new Claim("UserId",userDetails.Id.ToString()),
                    };

                    if (IsPatient)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Patient"));
                    }
                    // if (IsAdmin)
                    // {
                    //     claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    // }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    TempData["success"] = "Login Successfully";
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            TempData["error"] = "Logged In Failed";
            return View(nameof(Index), userView);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return View();
        }
    }
}