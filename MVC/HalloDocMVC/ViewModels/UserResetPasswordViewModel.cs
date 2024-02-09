using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocMVC.ViewModels
{
    public class UserResetPasswordViewModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;
    }
}