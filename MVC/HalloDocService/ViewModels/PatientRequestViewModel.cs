using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class PatientRequestViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is Invalid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mobile is required.")]
        [Phone(ErrorMessage = "Invalid Mobile number")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int? State { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public string? Zipcode { get; set; }

        // For Request Table
        public string? Symptoms { get; set; }

         [Required(ErrorMessage = "Birth Date is required.")]
         [DisplayName("Birth Date")]
         public string? Birthdate { get; set; }

        [StringLength(250)]
        public string? Roomnoofpatient { get; set; }

        // For Asp .net User Table
        [DisplayName("Password")]
        public string? Passwordhash { get; set; } = null!;
        
        public string? ConfirmPassword { get; set; }
        public string? FilePath { get; set; }

        public IEnumerable<RegionList> AllRegionList {get; set;} = new List<RegionList>();

        public int CreatedById {get; set;}

    }

}

