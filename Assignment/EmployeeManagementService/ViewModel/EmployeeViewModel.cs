using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.ViewModels
{
    public class EmployeeViewModel
    { 
       public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain alphabetic characters.")]
       public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain alphabetic characters.")]
       public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
       public string? Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(0,100,ErrorMessage = "Age must be between 0 and 100")]
       public int? Age { get; set; }

       [Required(ErrorMessage = "Gender is required")]
       public int? Gender { get; set; }
       [Required(ErrorMessage = "Department is required")]
       public int? DepartmentId { get; set; }
       public DepartmentViewModel? DepartmentDetails { get; set; }

       [Required(ErrorMessage = "Education is required")]
       public int? Education { get; set; }
       public string? EducationName { get; set; }

       [Required(ErrorMessage = "Company is required")]
       public string? Company { get; set; }

       [Required(ErrorMessage = "Experience is required")]
       public int? Experience { get; set; }

       [Required(ErrorMessage = "Package is required")]
       public decimal? Package { get; set; }

        public IEnumerable<DepartmentViewModel> AllDepartmentList {get; set;} = new List<DepartmentViewModel>();

    }

}