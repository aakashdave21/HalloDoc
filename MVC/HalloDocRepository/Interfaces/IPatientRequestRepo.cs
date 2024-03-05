using HalloDocRepository.DataModels;

namespace HalloDocRepository.Interfaces;
public interface IPatientRequestRepo
{
    Aspnetuser FindUserByEmail(string email);
    User FindUserByEmailFromUser(string email);

    void AddRequestDataForExistedUser(Request requestData);
    void AddPatientInfoForExistedUser(Requestclient requestData);
    void NewAspUserAdd(Aspnetuser requestData);
    void NewUserAdd(User requestData);
    void NewRequestAdd(Request requestData);
    void NewPatientAdd(Requestclient requestData);
    void ConciergeDetailsAdd(Concierge requestData);
    void RequestConciergeMappingAdd(Requestconcierge requestData);
    void AddDocumentDetails(Requestwisefile requestData);
    void StoreActivationToken(int AspUserId , string token, DateTime expiry);
}
