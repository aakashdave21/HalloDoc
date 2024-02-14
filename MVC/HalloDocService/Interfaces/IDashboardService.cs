
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IDashboardService
{
    User GetUserByEmail(string email);
    IEnumerable<Request> GetUserRequest(int userId);
    string GetUserRequestType(int status);

    User GetUserData(int userId);

    void EditUserProfile(int id,UserProfileViewModel userData);

    IEnumerable<Requestwisefile> GetAllRequestedDocuments (int reqId);

    void UploadFileFromDocument(string filename,int reqId);
}
