using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class AdminPhysicianEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }        
        public string? Password { get; set; }
        public int? StatusId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int? RoleId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain alphabetic characters.")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain alphabetic characters.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Medical License is required")]
        public string? MedicalLicense { get; set; }
        public string? SyncEmail { get; set; }
        [Required(ErrorMessage = "NPI is required")]
        public string? NPI { get; set; }

        [Required(ErrorMessage = "Address1 is required")]
        public string? Address1 { get; set; }
        [Required(ErrorMessage = "Address2 is required")]
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public int? State { get; set; }
        [Required(ErrorMessage = "ZIP Code is required")]
        [MaxLength(10, ErrorMessage = "ZIP Code must not exceed 10 characters")]
        [MinLength(5, ErrorMessage = "ZIP Code must be at least 5 characters long")]
        [RegularExpression(@"^\d+$", ErrorMessage = "ZIP Code must contain only numeric characters.")]
        public string? Zipcode { get; set; }
        [Required(ErrorMessage = "Alternate Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Alternate Phone Number")]
        public string? AltPhone { get; set; }
        [Required(ErrorMessage = "Business name is required")]
        public string? Businessname { get; set; }
        [Required(ErrorMessage = "Business Website is required")]
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