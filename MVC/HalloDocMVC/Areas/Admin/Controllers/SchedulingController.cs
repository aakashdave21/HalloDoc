using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SchedulingController : Controller
{ 

    private readonly IScheduleService _scheduleService;
    public SchedulingController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }


    public IActionResult Index(string startDate = ""){
        try
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate);
            return View(ScheduleList);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index","Dashboard");
        }
    }

    public IActionResult GetDayWiseData(string startDate = "",string? Status = null){
         try
        {
            Console.WriteLine(Status + "-------------------------------------");
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate,Status);
            return PartialView("_DayWiseCalendar",ScheduleList);
        }
        catch (Exception e)
        {
            return BadRequest(new {message = e});
        }
    }
}