using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class SchedulingController : Controller
{
    private readonly IScheduleService _scheduleService;
    public SchedulingController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

  public IActionResult Index(string startDate = "", string endDate = "", string? Status = null)
{
    try
    {
        if (string.IsNullOrEmpty(startDate))
        {
            startDate = DateTime.Today.ToString("yyyy-MM-01");
        }

        if (string.IsNullOrEmpty(endDate))
        {
            DateTime endOfMonth = new(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            endDate = endOfMonth.ToString("yyyy-MM-dd");
        }

        DateTime monthDate = DateTime.Parse(startDate);
        DayOfWeek dayOfWeek = monthDate.DayOfWeek;
        int dayOfWeekInt = (int)dayOfWeek;
        ViewBag.NoOfDays = dayOfWeekInt;

        int year = monthDate.Year;
        int month = monthDate.Month;
        ViewBag.lastDayOfMonth = DateTime.DaysInMonth(year, month);
        int PhyId = int.Parse(User.FindFirstValue("UserId"));
        SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate, endDate, Status,PhyId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_MonthWiseCalendar", ScheduleList);
        }
        return View(ScheduleList);
    }
    catch (Exception e)
    {
        return BadRequest(new { message = e });
    }
}
    [HttpPost]
    public IActionResult CreateShift(SchedulingViewModel scheduleView)
    {
        int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
        scheduleView.Physicianid = int.Parse(User.FindFirstValue("UserId"));
        _scheduleService.AddShift(scheduleView, AspUserId, 2);
        return RedirectToAction("Index");
    }
}