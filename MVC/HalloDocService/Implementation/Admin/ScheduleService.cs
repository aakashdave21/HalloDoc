using System.Globalization;
using HalloDocRepository.Admin.Implementation;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.DataModels;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.Admin.Implementation;
public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepo _scheduleRepo;
    private readonly IProviderRepo _providerRepo;
    private readonly IProfileRepo _profileRepo;
    public ScheduleService(IScheduleRepo scheduleRepo, IProviderRepo providerRepo, IProfileRepo profileRepo)
    {
        _scheduleRepo = scheduleRepo;
        _providerRepo = providerRepo;
        _profileRepo = profileRepo;
    }

    public SchedulingViewModel ShiftsLists(string startDate, string endDate, string? status = null,int? PhyId=null)
    {
        SchedulingViewModel scheduleView = new()
        {
            AllShiftList = _scheduleRepo.ShiftsLists(startDate, endDate, status, PhyId).Select(sd => new ShiftDetailsInfo()
            {
                Id = sd.Id,
                ProviderId = sd.Shift.Physicianid,
                ShiftDetailId = sd?.Id,
                StartDate = sd?.Shiftdate.ToString("yyyy-MM-dd"),
                StartTimeHour = sd?.Starttime.Hour,
                StartTimeMinute = sd?.Starttime.Minute,
                EndTimeHour = sd?.Endtime.Hour,
                EndTimeMinute = sd?.Endtime.Minute,
                FullName = sd?.Shift.Physician.Firstname + ", " + sd?.Shift.Physician.Lastname,
                RegionName = sd?.Region?.Name,
                Status = sd?.Status,
            }).ToList(),
            RepeatDaysList = Enumerable.Range(1, 7).Select(i => new RepeatDays
            {
                Id = i,
                DayName = ((DayOfWeek)i - 1).ToString(),
                IsSelected = false
            }).ToList()
        };
        if(PhyId!=null){
            scheduleView.AllProvidersList = new();
            scheduleView.AllRegions = _scheduleRepo.GetRegionByPhysician(PhyId).Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }).ToList();
        }else{
            scheduleView.AllProvidersList = _providerRepo.GetAllPhysicianList().Select(phy => new ProviderList()
            {
                Id = phy.Id,
                FullName = phy?.Firstname + " " + phy?.Lastname,
                PhotoPath = phy?.Photo != null ? "uploads/" + Path.GetFileName(phy.Photo) : null
            }).ToList();
            scheduleView.AllRegions = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }).ToList();
        }
        foreach (var repeatDay in scheduleView.RepeatDaysList)
        {
            repeatDay.DayName = "Every " + repeatDay.DayName;
        }
        return scheduleView;
    }

    public void AddShift(SchedulingViewModel schedulingView, int AspUserId, short AccountType = 1)
    {
        Shift newShift = new()
        {
            Physicianid = schedulingView.Physicianid,
            Startdate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null),
            Isrepeat = schedulingView.IsRepeat,
            Repeatupto = schedulingView.RepeatTime,
            Createdby = AspUserId
        };
        _scheduleRepo.CreateShift(newShift);
        if (schedulingView.IsRepeat == false)
        {
            DateOnly Startdate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null);
            Shiftdetail newShiftDetails = new()
            {
                Shiftid = newShift.Id,
                Shiftdate = new DateTime(Startdate.Year, Startdate.Month, Startdate.Day, 0, 0, 0),
                Regionid = schedulingView.RegionId,
                Starttime = TimeOnly.ParseExact(schedulingView?.StartTime, "HH:mm", null),
                Endtime = TimeOnly.ParseExact(schedulingView?.EndTime, "HH:mm", null),
                Status = (short)((AccountType == 1) ? 2 : 1),
                Createdby = AspUserId
            };
            _scheduleRepo.CreateShiftDetails(newShiftDetails);
        }
        else
        {
            List<Shiftdetail> shiftDetailsList = new();

            for (int i = 0; i < schedulingView?.RepeatTime; i++)
            {
                DateOnly shiftDate = DateOnly.ParseExact(schedulingView?.ShiftDate, "yyyy-MM-dd", null);
                int StatrtDay = shiftDate.Day;
                foreach (var item in schedulingView.RepeatDaysList)
                {
                    if (item.IsSelected)
                    {
                        DateOnly shiftDates;
                        if (item.Id - 1 >= StatrtDay) shiftDates = shiftDate.AddDays((item.Id - 1) - StatrtDay + (i * 7));
                        else shiftDates = shiftDate.AddDays((7 - StatrtDay) + (item.Id - 1) + (i * 7));
                        Shiftdetail newShiftDetail = new()
                        {
                            Shiftid = newShift.Id,
                            Shiftdate = new DateTime(shiftDates.Year, shiftDates.Month, shiftDates.Day, 0, 0, 0),
                            Regionid = schedulingView?.RegionId,
                            Starttime = TimeOnly.ParseExact(schedulingView?.StartTime ?? "", "HH:mm", null),
                            Endtime = TimeOnly.ParseExact(schedulingView?.EndTime ?? "", "HH:mm", null),
                            Status = (short)((AccountType == 1) ? 2 : 1),
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

    public void UpdateSchedule(IFormCollection? formData, int aspUserId)
    {
        int providerId = int.Parse(formData["providerId"]);
        int shiftId = int.Parse(formData["shiftId"]);
        string? shiftDate = formData["shiftDate"];
        TimeOnly startTime = TimeOnly.FromDateTime(DateTime.ParseExact(formData["startTime"], "HH:mm", CultureInfo.InvariantCulture));
        TimeOnly endTime = TimeOnly.FromDateTime(DateTime.ParseExact(formData["endTime"], "HH:mm", CultureInfo.InvariantCulture));
        IEnumerable<Shiftdetail> shiftList = _scheduleRepo.GetShiftByProviderId(providerId);

        var existingShift = shiftList.Where(shift => shift.Isdeleted == false && shift.Id != shiftId && shift.Shiftdate.ToString("yyyy-MM-dd") == shiftDate && (startTime >= shift.Starttime && startTime <= shift.Endtime || endTime >= shift.Starttime && endTime <= shift.Endtime));
        if (existingShift.Any())
        {
            var ex = new Exception("A shift with this time already exists.");
            ex.Data["IsShiftOverlap"] = true;
            throw ex;
        }
        else
        {
            _scheduleRepo.UpdateShift(shiftId, shiftDate!=null ? DateTime.Parse(shiftDate) : DateTime.MinValue, startTime, endTime, aspUserId);
        }
    }

    public void ChangeStatus(int shiftId,int AspUserId){
        _scheduleRepo.ChangeStatus(shiftId,AspUserId);
    }
    public void Delete(int shiftId,int AspUserId){
        _scheduleRepo.Delete(shiftId,AspUserId);
    }

    public SchedulingViewModel GetCallPhysician(int regionId = 0){
        IEnumerable<Physician> physicianList = _scheduleRepo.GetCallPhysician(regionId);
        SchedulingViewModel listView = new(){
            AllRegions = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }).ToList(),
            OnCallPhysicianList = physicianList.Where(phy => phy.OnCallStatus == 2).Select(phy => new ProviderList(){
                Id = phy.Id,
                FullName = phy.Firstname + " " + phy.Lastname,
                PhotoPath = phy?.Photo != null ? "uploads/" + Path.GetFileName(phy.Photo) : null,
                OnCallStatus = phy?.OnCallStatus.ToString()
            }),
            UnAvailablePhysicianList = physicianList.Where(phy => phy.OnCallStatus == 1).Select(phy => new ProviderList(){
                Id = phy.Id,
                FullName = phy.Firstname + " " + phy.Lastname,
                PhotoPath = phy?.Photo != null ? "uploads/" + Path.GetFileName(phy.Photo) : null,
                OnCallStatus = phy?.OnCallStatus.ToString()
            }),
        };
        return listView;
    }

    public SchedulingViewModel ReviewShift(int RegionId=0,int PageSize=5,int PageNum=1){
        ReviewShiftPage shiftInfo = _scheduleRepo.ReviewShift(RegionId,PageSize,PageNum);
        int startIndex = (PageNum - 1) * PageSize + 1;

        SchedulingViewModel schedulingView = new(){
            AllRegions = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }).ToList(),
            AllReviewList = shiftInfo.ShiftListsInfo.Select(sd => new ShiftDetailsInfo(){
                ShiftDetailId = sd.Id,
                Id = sd.Id,
                FullName = sd.Shift.Physician.Firstname + " " + sd.Shift.Physician.Lastname,
                ProviderId = sd.Shift.Physicianid,
                StartDate = sd?.Shiftdate.ToString("yyyy-MM-dd"),
                StartTimeHour = sd?.Starttime.Hour,
                StartTimeMinute = sd?.Starttime.Minute,
                EndTimeHour = sd?.Endtime.Hour,
                EndTimeMinute = sd?.Endtime.Minute,
                RegionName = sd?.Region?.Name
            }).ToList(),
            TotalReview = shiftInfo.TotalPage,
            CurrentPage = PageNum,
            CurrentPageSize = PageSize,
            PageRangeStart = shiftInfo.TotalPage == 0 ? 0 : startIndex,
            TotalPage = (int)Math.Ceiling((double)shiftInfo.TotalPage / PageSize),
            PageRangeEnd = Math.Min(startIndex + PageSize - 1, shiftInfo.TotalPage)
        };

        return schedulingView;
    }

    public void UpdateShift(List<int> shiftDetailIds,int AspUserId,string IsDelete){
        _scheduleRepo.UpdateShift(shiftDetailIds,AspUserId,IsDelete);
    }

}