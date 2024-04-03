using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class SchedulingViewModel
    { 
         [Required(ErrorMessage = "RegionId is required")]
        public int RegionId { get; set; }

        [Required(ErrorMessage = "Physicianid is required")]
        public int Physicianid { get; set; }

        [Required(ErrorMessage = "ShiftDate is required")]
        public string? ShiftDate { get; set; }

        [Required(ErrorMessage = "StartTime is required")]
        public string? StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required")]
        public string? EndTime { get; set; }
        public bool IsRepeat { get; set; } = false;
        public int RepeatTime { get; set; }
        public List<RepeatDays> RepeatDaysList {get; set;} = new();
        public List<ProviderList> AllProvidersList { get; set; } = new();
        public List <ShiftDetailsInfo> AllShiftList { get; set; } = new();
        public List<RegionList> AllRegions {get; set;} = new();
    }

    public class ProviderList {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhotoPath { get; set; }
        public bool? IsActive { get; set; } = false;
    }

    public class ShiftDetailsInfo{
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string? RegionName {get; set;}
        public string? FullName { get; set; }

        public int? ShiftDetailId { get; set; }
        public int? StartTimeHour {get; set;}
        public int? StartTimeMinute {get; set;}
        public int? EndTimeHour {get; set;}
        public int? EndTimeMinute {get; set;}
        public string? StartDate { get; set; }
        public short? Status { get; set; }
    }

    public class RepeatDays{
        public int Id { get; set; }
        public string? DayName { get; set; }
        public bool IsSelected { get; set; }
    }
}