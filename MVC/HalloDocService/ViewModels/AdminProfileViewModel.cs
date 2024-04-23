using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminProfileViewModel
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }
        public int Status { get; set; }
        public int? Role { get; set; }

        // Administrator Information
        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Confirm email is required.")]
        [Compare("Email", ErrorMessage = "Emails do not match")]
        public string? ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Mobile is required.")]
        public string? Mobile { get; set; }
        public bool[]? CheckBoxes { get; set; }

        [Required(ErrorMessage = "Address1 is required.")]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int? State { get; set; }

        [Required(ErrorMessage = "ZIP Code is required")]
        [MaxLength(10, ErrorMessage = "ZIP Code must not exceed 10 characters")]
        [MinLength(5, ErrorMessage = "ZIP Code must be at least 5 characters long")]
        [RegularExpression(@"^\d+$", ErrorMessage = "ZIP Code must contain only numeric characters.")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string? Phone { get; set; }

        public IEnumerable<RoleList> roleSelect { get; set; } = new List<RoleList>();
        public IEnumerable<RegionList> regionSelect { get; set; } = new List<RegionList>();
        public IEnumerable<RegionList> RegionUnSelect { get; set; } = new List<RegionList>();
        public IEnumerable<RegionList> AllRegionList { get; set; } = new List<RegionList>();

        public List<int> UnCheckedRegions { get; set; } = new List<int>();
        public List<int> CheckedRegions { get; set; } = new List<int>();
    }

    public class RoleList
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class RegionList
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}