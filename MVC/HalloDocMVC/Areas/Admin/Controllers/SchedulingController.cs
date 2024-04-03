using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
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


    public IActionResult Index(string startDate = "")
    {
        try
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate, startDate);
            return View(ScheduleList);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    public IActionResult GetDayWiseData(string startDate = "", string? Status = null)
    {
        try
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate, startDate, Status);
            return PartialView("_DayWiseCalendar", ScheduleList);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
    public IActionResult GetWeekWiseData(string startDate = "", string endDate = "", string? Status = null)
    {
        try
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate, endDate, Status);
            return PartialView("_WeekWiseCalendar", ScheduleList);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
    public IActionResult GetMonthWiseData(string startDate = "", string endDate = "", string? Status = null)
    {
        try
        {
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Today.ToString("yyyy-MM-dd");
            }
            DateTime monthDate = DateTime.Parse(startDate);

            DayOfWeek dayOfWeek = monthDate.DayOfWeek;
            int dayOfWeekInt = (int)dayOfWeek;
            ViewBag.NoOfDays = dayOfWeekInt;

            int year = monthDate.Year;
            int month = monthDate.Month;
            ViewBag.lastDayOfMonth = DateTime.DaysInMonth(year, month);

            SchedulingViewModel ScheduleList = _scheduleService.ShiftsLists(startDate, endDate, Status);
            return PartialView("_MonthWiseCalendar", ScheduleList);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
    public IActionResult ChangeStatus(string ShiftId)
    {
        try
        {
            if (string.IsNullOrEmpty(ShiftId))
            {
                throw new Exception();
            }
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            _scheduleService.ChangeStatus(int.Parse(ShiftId), AspUserId);
            return Ok(new { message = "Successfully Changed !" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }
    public IActionResult Delete(string ShiftId)
    {
        try
        {
            if (string.IsNullOrEmpty(ShiftId))
            {
                throw new Exception();
            }
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            _scheduleService.Delete(int.Parse(ShiftId), AspUserId);
            return Ok(new { message = "Successfully Deleted !" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e });
        }
    }

    [HttpPost]
    public IActionResult Edit(IFormCollection? formData)
    {
        try
        {
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            _scheduleService.UpdateSchedule(formData, AspUserId);
            return Ok(new {message = "Successfully updated"});
        }
        catch (Exception ex)
        {
            if (ex.Data.Contains("IsShiftOverlap") && (bool)ex.Data["IsShiftOverlap"])
            {
                return BadRequest(new {message = ex.Message});
            }
            return BadRequest(new {message = "Internal Server Error"});
        }
    }

    [HttpPost]
    public IActionResult CreateShift(SchedulingViewModel scheduleView)
    {
        int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
        _scheduleService.AddShift(scheduleView, AspUserId);
        return RedirectToAction("Index");
    }
}