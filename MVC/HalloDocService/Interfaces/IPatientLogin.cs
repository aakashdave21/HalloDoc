
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IPatientLogin
{
    Aspnetuser ValidateUser(UserLoginViewModel userLoginData);
    bool VerifyPassword(string userPassword, string storedHashPassword);

    Aspnetuser FindUserFromUsername(UserResetPasswordViewModel user);

    User UserDetailsFetch(string email); 
}
