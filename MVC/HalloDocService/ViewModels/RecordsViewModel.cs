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
        public IEnumerable<RecordsView> RecordsRequestList = new List<RecordsView>();
        public IEnumerable<EmailLogsView> EmailLogsList = new List<EmailLogsView>();
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

    public class RecordsView
    {
        public int RequestId { get; set; }
        public string? PatientName { get; set; }
        public string? Requestor { get; set; }
        public string? DateOfService { get; set; }
        public string? CloseCaseDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Zip { get; set; }
        public string? RequestStatus { get; set; }
        public int? PhysicianId { get; set; }
        public string? PhysicianName { get; set; }
        public string? PhysicianNote { get; set; }
        public string? CancelledByProviderNote { get; set; }
        public string? AdminNote { get; set; }
        public string? PatientNote { get; set; }

        public int InputRequestStatus {get; set;}
        public int InputRequestType {get; set;}
        public DateOnly InputFromDate {get; set;}
        public DateOnly InputToDate {get; set;}
    }

    public class EmailLogsView
    {
        public int Id { get; set; }
        public string? Recipient { get; set; }
        public string? Action { get; set; }
        public string? RoleName { get; set; }
        public string? Email { get; set; }
        public string? CreatedDate { get; set; }
        public string? SentDate { get; set; }
        public bool? IsSent { get; set; } = false;
        public int SentTries { get; set; } = 0;
        public string? ConfirmationNumber { get; set; }
    }
}