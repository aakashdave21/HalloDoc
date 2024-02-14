using HalloDocRepository.DataModels;

namespace HalloDocRepository.Interfaces;
public interface IPatientLoginRepo
{
    Aspnetuser ValidateUser(string email);
    Aspnetuser userDetailsFromUserName(string username);

    User UserDetailsFetch(string email);
}
