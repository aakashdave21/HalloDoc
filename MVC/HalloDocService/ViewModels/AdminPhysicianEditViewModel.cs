using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class AdminPhysicianEditViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? StatusId { get; set; }
        public int? RoleId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? MedicalLicense { get; set; }
        public string? NPI { get; set; }
        public string? SyncEmail { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public int? State { get; set; }
        public string? Zipcode { get; set; }
        public string? AltPhone { get; set; }
        public string? Businessname { get; set; }
        public string? BusinessWebsite { get; set; }
        public string? PhotoFileName { get; set; }
        public string? SignFileName { get; set; }
        public string? AdminNote { get; set; }
        public bool? IsICA { get; set; } = false;
        public bool? IsBgCheck { get; set; } = false;
        public bool? IsHIPAA { get; set; } = false;
        public bool? IsNDA { get; set; } = false;
        public bool? IsLicenseDoc { get; set; } = false;
        public string? IsICAFile { get; set; } 
        public string? IsBgCheckFile { get; set; }
        public string? IsHIPAAFile { get; set; }
        public string? IsNDAFile { get; set; }
        public string? IsLicenseDocFile { get; set; }
        public IEnumerable<RoleList> AllRoleList = new List<RoleList>();
        public IEnumerable<RegionList> AllRegionList = new List<RegionList>();

         public IEnumerable<RegionList> RegionSelect {get; set;} = new List<RegionList>();
        public IEnumerable<RegionList> RegionUnSelect {get; set;} = new List<RegionList>();
        public IFormFile? UserPhoto { get; set; }
        public IFormFile? UserSign { get; set; }

        public string? UploadPhoto {get; set;}
        public string? UploadSign {get; set;}
     }
}