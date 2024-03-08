using System.ComponentModel;
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

        public int TotalPage {get; set;}
        
        public int PageRangeStart { get; set; }
        public int PageRangeEnd { get; set; }
        public int NoOfPage { get; set; }

        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();

        public IEnumerable<Region>? RegionList {get;set;}
    }

    public class RequestViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string BirthDate { get; set; }

        public string Requestor { get; set; }
        public string RequestedDate {get; set;}
        public string Address {get; set;}
        public string Notes {get; set;}
        public int RequestType {get; set;}
        public string PhysicianName {get; set;}
        public int? PhysicianId {get; set;}
        public string ServiceDate {get; set;}
        public string Region {get; set;}
       
    }

}