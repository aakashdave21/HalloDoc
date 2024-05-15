using System.ComponentModel.DataAnnotations;
using HalloDocRepository.DataModels;

namespace HalloDocService.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int NewState { get; set; } = 0;
        public int PendingState { get; set; } = 0;
        public int ActiveState { get; set; } = 0;
        public int ConcludeState { get; set; } = 0;
        public int ToCloseState { get; set; } = 0;
        public int UnPaidState { get; set; } = 0;
        public int TotalPage { get; set; }
        public int PageRangeStart { get; set; }
        public int PageRangeEnd { get; set; }
        public int NoOfPage { get; set; }
        public SendMailViewModel? SendMailModal { get; set; } = null;
        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();
        public IEnumerable<Region>? RegionList { get; set; }
    }

    public class RequestViewModel
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? BirthDate { get; set; }
        public string? Requestor { get; set; }
        public string? RequestedDate { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int RequestType { get; set; }
        public string? PhysicianName { get; set; }
        public int? PhysicianId { get; set; }
        public string? ServiceDate { get; set; }
        public string? Region { get; set; }
        public bool? IsFinalized { get; set; } = false;
        public int? PhysicianAspId {get; set;}
        public int? PatientAspId {get; set;}

    }

    public class HeaderMenu
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public int? AccountType { get; set; }
        public int? SortOrder { get; set; }
    }

    public class SendMailViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain alphabetic characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain alphabetic characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        public string? Mobile { get; set; }
    }

}