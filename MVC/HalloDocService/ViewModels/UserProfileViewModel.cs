using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class UserProfileViewModel
    {   
        [Required(ErrorMessage = "First Name is Required!")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Last Name is Required!")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        [DisplayName("Birth Date")]
        public string? Birthdate { get; set; }

        public string? Type {get; set;}

        [Required(ErrorMessage = "Mobile is required.")]
        [Phone(ErrorMessage = "Invalid Mobile number")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is Invalid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public string? Zipcode { get; set; }

    }
}