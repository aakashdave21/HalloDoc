using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class BusinessRequestViewModel
    {

        // Family Request
         
        [Required(ErrorMessage = "Your First Name is required.")]
        public string? BusinessFirstname { get; set; }
        
        public string? BusinessLastname { get; set; }

       [Required(ErrorMessage = "Your Mobile Name is required.")]
        public string? BusinessPhonenumber { get; set; }

       [Required(ErrorMessage = "Your Email is required.")]
        public string? BusinessEmail { get; set; }

        [Required(ErrorMessage = "Property Name is required. ")]
        public string? PropertyName {get; set;}

        public string? CaseNumber {get; set;}

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
        public string? State { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public string? Zipcode { get; set; }

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