using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult AccessDenied(){
        return View();
    }
    public IActionResult NotFound(){
        return View();
    }
}
