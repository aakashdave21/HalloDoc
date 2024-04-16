
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using AdminTable = HalloDocRepository.DataModels.Admin;


namespace HalloDocService.Interfaces;
public interface IPatientLogin
{
    Aspnetuser ValidateUser(UserLoginViewModel userLoginData);
    bool VerifyPassword(string userPassword, string storedHashPassword);

    Aspnetuser FindUserFromUsername(UserResetPasswordViewModel user);

    User UserDetailsFetch(string email); 
    AdminTable AdminDetailsFetch(string email); 
    Physician ProviderDetailsFetch(string email); 

    void StoreResetToken(int AspUserId, string token, DateTime expiry);
    Aspnetuser? GetResetTokenExpiry(int AspUserId, string token);

    void UpdatePassword(int AspUserId, string password);

    string GetAspUserEmail(int userId);

}
