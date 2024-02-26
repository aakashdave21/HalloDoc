using HalloDocRepository.DataModels;

namespace HalloDocRepository.Interfaces;
public interface IPatientLoginRepo
{
    Aspnetuser ValidateUser(string email);
    Aspnetuser userDetailsFromUserName(string username);

    User UserDetailsFetch(string email);
    void StoreResetToken(int AspUserId, string token, DateTime expiry);

    Aspnetuser? GetResetTokenExpiry(int AspUserId, string token);
    void UpdatePassword(int userId , string password);
}
