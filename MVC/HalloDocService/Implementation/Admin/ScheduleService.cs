using System.Globalization;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.DataModels;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;

namespace HalloDocService.Admin.Implementation;
public class ScheduleService : IScheduleService
{ 
    private readonly IScheduleRepo _scheduleRepo;
    private readonly IProviderRepo _providerRepo;
    public ScheduleService(IScheduleRepo scheduleRepo, IProviderRepo providerRepo)
    {
        _scheduleRepo = scheduleRepo;
        _providerRepo = providerRepo;
    }

    public SchedulingViewModel ShiftsLists(string startDate,string? status = null){
        SchedulingViewModel scheduleView = new(){
            AllShiftList = _scheduleRepo.ShiftsLists(startDate,status).Select(sd => new ShiftDetails(){
                Id = sd.Id,
                ProviderId = sd.Physicianid,
                ShiftDetailId = sd?.Shiftdetail?.Id,
                StartDate = sd?.Startdate.ToString(),
                StartTimeHour = sd?.Shiftdetail?.Starttime.Hour,
                StartTimeMinute = sd?.Shiftdetail?.Starttime.Minute,
                EndTimeHour = sd?.Shiftdetail?.Endtime.Hour,
                EndTimeMinute = sd?.Shiftdetail?.Endtime.Minute,
                Status = sd?.Shiftdetail?.Status,
            }).ToList(),
            AllProvidersList = _providerRepo.GetAllPhysician(true,null).Select(phy => new ProviderList(){
                Id = phy.Id,
                FullName = phy?.Firstname + " " + phy?.Lastname,
                PhotoPath = phy?.Photo != null ? "uploads/" + Path.GetFileName(phy.Photo) : null
            }).ToList()
        };
        return scheduleView;
    }
}