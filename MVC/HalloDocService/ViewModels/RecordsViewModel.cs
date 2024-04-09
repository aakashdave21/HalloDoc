using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class RecordsViewModel
    {
        public IEnumerable<PatientHistoryView> PatientHistoryList = new List<PatientHistoryView>();
        public IEnumerable<PatientRequestView> PatientRequestList = new List<PatientRequestView>();
        public int TotalCount {get; set;}
        public int CurrentPage {get; set;}
        public int CurrentPageSize {get; set;}
        public int PageRangeStart {get; set;}
        public int PageRangeEnd {get; set;}
        public int TotalPage {get; set;}
    }

    public class PatientHistoryView
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RequestId { get; set; }
        public string? Firstname  { get; set; } = "";
        public string? Lastname  { get; set; } = "";
        public string? Email  { get; set; } = "";
        public string? PhoneNumber  { get; set; } = "";
        public string? Address  { get; set; } = "";
    }

    public class PatientRequestView
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? CreatedDate { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string? ProviderName { get; set; }
        public string? ConcludedDate { get; set; }
        public string? Status { get; set; }
        public string? FinalReport { get; set; }
        public string? NoOfDocument { get; set; }
    }
}