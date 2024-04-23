using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminAccountViewModel
{
    public int? CreatedUser { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int? RoleId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain alphabetic characters.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain alphabetic characters.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Email Confirmation is required")]
    [Compare("Email", ErrorMessage = "Email and Email Confirmation must match")]
    public string? EmailConfirmation { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string? City { get; set; }

    [Required(ErrorMessage = "State is required")]
    public int? State { get; set; }

    [Required(ErrorMessage = "ZIP Code is required")]
    [MaxLength(10, ErrorMessage = "ZIP Code must not exceed 10 characters")]
    [MinLength(5, ErrorMessage = "ZIP Code must be at least 5 characters long")]
    [RegularExpression(@"^\d+$", ErrorMessage = "ZIP Code must contain only numeric characters.")]
    public string? Zip { get; set; }

    [Required(ErrorMessage = "Alternate Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Alternate Phone Number")]

    public string? AlternatePhone { get; set; }

    public IEnumerable<RoleList> AllRoleList { get; set; } = new List<RoleList>();
    public List<AllRegionList> AllRegionsList { get; set; } = new List<AllRegionList>();
    public List<AllRegionList> AllCheckBoxRegionList { get; set; } = new List<AllRegionList>();
    

}

    public class AllRegionList{
        public int Id {get; set;}
        public string? Name {get; set;}
        public bool IsSelected { get; set; } = false;

    }
}