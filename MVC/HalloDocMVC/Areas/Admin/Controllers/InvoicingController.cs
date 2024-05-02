using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class InvoicingController : Controller
{
    public InvoicingController(){
    }

    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            return View();
        }
    }

    public IActionResult Timesheet(){
        try
        {
            return View();
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}