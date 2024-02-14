using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class ConciergeRequestViewModel
    {

        // Family Request
         
        [Required(ErrorMessage = "Your First Name is required.")]
        public string? ConciergeFirstname { get; set; }
        
        public string? ConciergeLastname { get; set; }

       [Required(ErrorMessage = "Your Mobile Name is required.")]
        public string? ConciergePhonenumber { get; set; }

       [Required(ErrorMessage = "Your Email is required.")]
        public string? ConciergeEmail { get; set; }

        [Required(ErrorMessage = "Property Name is required. ")]
        public string? PropertyName {get; set;}

        [Required(ErrorMessage = "Street is required.")]
        public string? ConciergeStreet { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? ConciergeCity { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string? ConciergeState { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public string? ConciergeZipcode { get; set; }

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

        // For Request Table
        [Required(ErrorMessage = "Symptoms is required.")]
        public string? Symptoms { get; set; }

         [Required(ErrorMessage = "Birth Date is required.")]
         [DisplayName("Birth Date")]
         public DateOnly? Birthdate { get; set; }

        [StringLength(250)]
        public string? Roomnoofpatient { get; set; }
       

    }
}