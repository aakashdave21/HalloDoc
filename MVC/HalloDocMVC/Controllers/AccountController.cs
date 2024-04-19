using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult AccessDenied(){
        return View();
    }
    public new IActionResult NotFound()
    {
        return View();
    }
}
