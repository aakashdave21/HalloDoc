using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class TimeSheetViewModel
    {
        public int Id { get; set; }
        public int Physicianid { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public string? Status { get; set; }
        public bool? Isfinalized { get; set; }

        public IEnumerable<ProviderList> ProviderLists { get; set; } = new List<ProviderList>();
        public List<TimeSheetDetailsView> TimesheetdetailsList { get; set; } = new List<TimeSheetDetailsView>();
        public List<TimesheetreimbursementView> TimesheetreimbursementsList { get; set; } = new List<TimesheetreimbursementView>();
    }

    public class TimeSheetDetailsView
    {
        public int Id { get; set; }
        public int Timesheetid { get; set; }
        public DateTime? Shiftdate { get; set; }
        public int? OnCallhours { get; set; } = 0;
        public int? Shifthours { get; set; } = 0;
        public int? Housecall { get; set; } = 0;
        public int? Phoneconsult { get; set; } = 0;
        public bool Isweekend { get; set; } = false;

    }

    public class TimesheetreimbursementView
    {
        public int Id { get; set; }
        public int Timesheetid { get; set; }
        public string? Item { get; set; }
        public int Amount { get; set; }
        public string? Bill { get; set; }
        public DateTime? Shiftdate { get; set; }

    }
}