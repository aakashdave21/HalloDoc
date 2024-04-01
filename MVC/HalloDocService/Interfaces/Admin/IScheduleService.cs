using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
namespace HalloDocService.Admin.Interfaces;
public interface IScheduleService
{ 
    SchedulingViewModel ShiftsLists(string startDate,string endDate,string? status = null);
    void AddShift(SchedulingViewModel schedulingView,int AspUserId);
}