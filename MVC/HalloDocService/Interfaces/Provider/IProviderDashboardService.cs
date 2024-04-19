using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HalloDocService.Provider.Interfaces;
public interface IProviderDashboardService
{
   (List<RequestViewModel>, int totalCount) GetDashboardRequests(string status,string searchBy,int reqType,int pageNumber,int pageSize,int AspId);
    Dictionary<string,int> CountRequestByType(int AspId);

    void AcceptRequest(int ReqId);
    void SetTransferCase(int ReqId,int PhysicianId, string Description);

    bool CheckEncounterFinalized(int ReqId);

    void ConcludeCare(int reqId, string providerNote,int? phyId);

    void FinalizeForm(int EncId, int ReqId);

    Aspnetuser? SendProfileRequest(int? PhyId);

}