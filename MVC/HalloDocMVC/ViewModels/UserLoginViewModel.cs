using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocMVC.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }  = null!;

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required.")]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        //     ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Passwordhash { get; set; }  = null!;

    }
}