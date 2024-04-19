using HalloDocRepository.DataModels;
using AdminTable = HalloDocRepository.DataModels.Admin;


namespace HalloDocRepository.Interfaces;
public interface IPatientLoginRepo
{
    Aspnetuser? ValidateUser(string email);
    Aspnetuser? userDetailsFromUserName(string username);

    User UserDetailsFetch(string email);
    AdminTable AdminDetailsFetch(string email);
    Physician ProviderDetailsFetch(string email);
    void StoreResetToken(int AspUserId, string token, DateTime expiry);

    Aspnetuser? GetResetTokenExpiry(int AspUserId, string token);
    void UpdatePassword(int userId , string password);
    string GetAspUserEmail(int userId);
}
