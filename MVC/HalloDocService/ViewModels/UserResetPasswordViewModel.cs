using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class UserResetPasswordViewModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;

        public string? Password {get; set;}
        public string? ConfirmPassword {get; set;}

        public int UserId {get; set;}

        public string? UserToken {get; set;}

    }
}