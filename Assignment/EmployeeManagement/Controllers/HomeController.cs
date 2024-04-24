using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagementService.Interface;
using EmployeeManagementService.ViewModels;

namespace EmployeeManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger,IHomeService homeService)
    {   
        _homeService = homeService;
        _logger = logger;
    }

    public IActionResult Index(int PageSize = 10, int PageNum = 1, string? SearchBy=null)
    {
        try
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EmployeeListPartial", _homeService.EmployeeList(PageSize,PageNum,SearchBy));
            }
            return View(_homeService.EmployeeList(PageSize,PageNum,SearchBy));
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return NotFound();
        }
    }

    public IActionResult GetAllDepartments(){
        try
        {
            return Json(new {data = _homeService.GetAllDepartments()});
        }
        catch (System.Exception e)
        {
            return BadRequest(new {message = e});
        }
    }

    public IActionResult GetSingleRecord(int Id){
        try
        {
            EmployeeViewModel singleEmployee = _homeService.GetSingleEmployee(Id);
            return Json(new {data = singleEmployee});
        }
        catch (System.Exception e)
        {
            return BadRequest(new {message = e});
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EmployeeViewModel employeeData){
        try
        {
            _homeService.Create(employeeData);
            TempData["success"] = "Record Created Successfully";
            return RedirectToAction("Index");
        }
        catch (System.Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult Delete(int Id){
        try
        {
            _homeService.Delete(Id);
            TempData["success"] = "Record Deleted Successfully";
            return RedirectToAction("Index");
        }
        catch (System.Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
