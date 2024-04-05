using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.AspNetCore.Http;
namespace HalloDocService.Admin.Interfaces;
public interface IScheduleService
{ 
    SchedulingViewModel ShiftsLists(string startDate,string endDate,string? status = null);
    void AddShift(SchedulingViewModel schedulingView,int AspUserId);

    void UpdateSchedule(IFormCollection? formData,int aspUserId);
    void ChangeStatus(int shiftId,int AspUserId);
    void Delete(int shiftId,int AspUserId);

    SchedulingViewModel GetCallPhysician(int regionId=0);
    SchedulingViewModel ReviewShift(int RegionId=0,int PageSize=5,int PageNum=1);

    void UpdateShift(List<int> shiftDetailIds,int AspUserId,string IsDelete);
}