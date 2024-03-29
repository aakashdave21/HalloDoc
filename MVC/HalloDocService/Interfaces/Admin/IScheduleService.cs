using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
namespace HalloDocService.Admin.Interfaces;
public interface IScheduleService
{ 
    SchedulingViewModel ShiftsLists(string startDate,string? status = null);
}