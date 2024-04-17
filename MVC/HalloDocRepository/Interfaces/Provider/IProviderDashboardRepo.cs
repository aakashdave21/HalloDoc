using HalloDocRepository.DataModels;

namespace HalloDocRepository.Provider.Interfaces;
public interface IProviderDashboardRepo
{
   (List<Request> req, int totalCount) GetDashboardRequests(string status,string searchBy,int reqType,int pageNumber,int pageSize,int AspId);
   Dictionary<string, int> CountRequestByType(int AspId); 

   void AcceptRequest(int ReqId);
    bool CheckEncounterFinalized(int ReqId);

    void FinalizeForm(int EncId, int ReqId);
}
