using HalloDocRepository.DataModels;

namespace HalloDocRepository.Interfaces;
public interface IDashboardRepo
{
    User GetUserByEmail(string email);
    IEnumerable<Request> GetUserRequest(int userId);

    User GetUserData(int userId);

    void EditUserProfile(int id , User userData);

    IEnumerable<Requestwisefile> GetAllRequestedDocuments(int reqId);
}
