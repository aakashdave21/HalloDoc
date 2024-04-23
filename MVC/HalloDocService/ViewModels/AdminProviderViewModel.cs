using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminProviderViewModel
    { 
        public IEnumerable<RegionList> AllRegionList {get; set;} = new List<RegionList>();
        public IEnumerable<PhysicianList> AllPhysicianList {get; set;} = new List<PhysicianList>();

        public List<string> StopNotificationIds { get; set; } = new List<string>();
        public List<string> StartNotificationIds { get; set; } = new List<string>();
        public int TotalCount {get; set;}
        public int CurrentPage {get; set;}
        public int CurrentPageSize {get; set;}
        public int PageRangeStart {get; set;}
        public int PageRangeEnd {get; set;}
        public int TotalPage {get; set;}
    }

    public class PhysicianList{
        public int Id { get; set; }
        public int PhyId  { get; set; }

        public bool IsNotificationStopped { get; set; } = false;

        public string? PhysicianName {get; set;}

        public string? Role { get; set; }
        public int RoleId{ get; set; }
        public string? OnCallStatus{ get; set; }
        public string? Status{ get; set; }

    }

    
}