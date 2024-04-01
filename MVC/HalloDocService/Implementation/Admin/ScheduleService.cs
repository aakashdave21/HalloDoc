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
    private readonly IProfileRepo _profileRepo;
    public ScheduleService(IScheduleRepo scheduleRepo, IProviderRepo providerRepo,IProfileRepo profileRepo)
    {
        _scheduleRepo = scheduleRepo;
        _providerRepo = providerRepo;
        _profileRepo = profileRepo;
    }

    public SchedulingViewModel ShiftsLists(string startDate,string endDate,string? status = null){
        SchedulingViewModel scheduleView = new(){
            AllShiftList = _scheduleRepo.ShiftsLists(startDate,endDate,status).Select(sd => new ShiftDetailsInfo(){
                Id = sd.Id,
                ProviderId = sd.Shift.Physicianid,
                ShiftDetailId = sd?.Id,
                StartDate = sd?.Shiftdate.ToString(),
                StartTimeHour = sd?.Starttime.Hour,
                StartTimeMinute = sd?.Starttime.Minute,
                EndTimeHour = sd?.Endtime.Hour,
                EndTimeMinute = sd?.Endtime.Minute,
                Status = sd?.Status,
            }).ToList(),
            AllProvidersList = _providerRepo.GetAllPhysician(true,null).Select(phy => new ProviderList(){
                Id = phy.Id,
                FullName = phy?.Firstname + " " + phy?.Lastname,
                PhotoPath = phy?.Photo != null ? "uploads/" + Path.GetFileName(phy.Photo) : null
            }).ToList(),
            AllRegions = _profileRepo.GetAllRegions().Select(reg => new RegionList(){
                Id = reg.Id,
                Name = reg.Name
            }).ToList(),
            RepeatDaysList = Enumerable.Range(1, 7).Select(i => new RepeatDays
            {
                Id = i,
                DayName = ((DayOfWeek)i - 1).ToString(),
                IsSelected = false
            }).ToList()
        };
        foreach (var repeatDay in scheduleView.RepeatDaysList)
        {
            repeatDay.DayName = "Every " + repeatDay.DayName;
        }
        return scheduleView;
    }

    public void AddShift(SchedulingViewModel schedulingView,int AspUserId){
        Shift newShift = new(){
            Physicianid = schedulingView.Physicianid,
            Startdate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null),
            Isrepeat = schedulingView.IsRepeat,
            Repeatupto = schedulingView.RepeatTime,
            Createdby = AspUserId
        };
        _scheduleRepo.CreateShift(newShift);
        if(schedulingView.IsRepeat == false){
            DateOnly Startdate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null);
            Shiftdetail newShiftDetails = new(){
                Shiftid = newShift.Id,
                Shiftdate = new DateTime(Startdate.Year, Startdate.Month, Startdate.Day, 0, 0, 0),
                Regionid = schedulingView.RegionId,
                Starttime = TimeOnly.ParseExact(schedulingView?.StartTime, "HH:mm", null),
                Endtime = TimeOnly.ParseExact(schedulingView?.EndTime, "HH:mm", null),
                Status = 1,
                Createdby = AspUserId
            };
            _scheduleRepo.CreateShiftDetails(newShiftDetails);
        }else{
            List<Shiftdetail> shiftDetailsList = new();
            
            for(int i=0;i<schedulingView.RepeatTime;i++)
            {
                DateOnly shiftDate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null);
                int StatrtDay = shiftDate.Day;
                foreach (var item in schedulingView.RepeatDaysList)
                {
                    if(item.IsSelected){
                        DateOnly shiftDates;
                        if (item.Id-1 >= StatrtDay) shiftDates = shiftDate.AddDays((item.Id-1) - StatrtDay+ (i  * 7));
                        else shiftDates = shiftDate.AddDays((7 - StatrtDay) + (item.Id-1) + (i * 7));
                        Shiftdetail newShiftDetail = new()
                        {
                            Shiftid = newShift.Id,
                            Shiftdate = new DateTime(shiftDates.Year, shiftDates.Month, shiftDates.Day, 0, 0, 0),
                            Regionid = schedulingView.RegionId,
                            Starttime = TimeOnly.ParseExact(schedulingView?.StartTime, "HH:mm", null),
                            Endtime = TimeOnly.ParseExact(schedulingView?.EndTime, "HH:mm", null),
                            Status = 1,
                            Createdby = AspUserId
                        };

                        shiftDetailsList.Add(newShiftDetail);
                    }
                    
                }
            }
            _scheduleRepo.CreateShiftDetailsList(shiftDetailsList);
        }
    }
    static DateTime GetNextDay(DateTime startDate, DayOfWeek targetDay)
    {
        int daysUntilTarget = ((int)targetDay - (int)startDate.DayOfWeek + 7) % 7;
        return startDate.AddDays(daysUntilTarget);
    }
}