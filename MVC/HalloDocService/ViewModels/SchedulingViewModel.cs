using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class SchedulingViewModel
    { 
        public List<ProviderList> AllProvidersList { get; set; } = new();
        public List <ShiftDetails> AllShiftList { get; set; } = new();
    }

    public class ProviderList {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhotoPath { get; set; }
        public bool? IsActive { get; set; } = false;
    }

    public class ShiftDetails{
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int? ShiftDetailId { get; set; }
        public int? StartTimeHour {get; set;}
        public int? StartTimeMinute {get; set;}
        public int? EndTimeHour {get; set;}
        public int? EndTimeMinute {get; set;}
        public string? StartDate { get; set; }
        public short? Status { get; set; }
    }
}