using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class AdminPhysicianCreateViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }
        public int? StatusId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int? RoleId { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "MedicalLicense is required")]
        public string? MedicalLicense { get; set; }
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
        [Required(ErrorMessage = "Zipcode is required")]
        public string? Zipcode { get; set; }
        [Required(ErrorMessage = "AltPhone is required")]
        public string? AltPhone { get; set; }
        [Required(ErrorMessage = "Businessname is required")]
        public string? Businessname { get; set; }
        [Required(ErrorMessage = "BusinessWebsite is required")]
        public string? BusinessWebsite { get; set; }
        public string? PhotoFileName { get; set; }
        public string? SignFileName { get; set; }
        public string? AdminNote { get; set; }
        public bool IsICA { get; set; } = false;
        public bool IsBgCheck { get; set; } = false;
        public bool IsHIPAA { get; set; } = false;
        public bool IsNDA { get; set; } = false;
        public bool IsLicenseDoc { get; set; } = false;
        public IFormFile? IsICAFile { get; set; }
        public IFormFile? IsBgCheckFile { get; set; }
        public IFormFile? IsHIPAAFile { get; set; }
        public IFormFile? IsNDAFile { get; set; }
        public IFormFile? IsLicenseDocFile { get; set; }
        public string? IsICAFileName { get; set; }
        public string? IsBgCheckFileName { get; set; }
        public string? IsHIPAAFileName { get; set; }
        public string? IsNDAFileName { get; set; }
        public string? IsLicenseDocFileName { get; set; }
        public IEnumerable<RoleList> AllRoleList = new List<RoleList>();
        public List<AllRegionList> AllRegionsList = new();
        public List<AllRegionList> AllCheckBoxRegionList = new();
        public IFormFile? UserPhoto { get; set; }
        public string? UploadPhoto { get; set; }
        public string? UploadSign { get; set; }
    }
}