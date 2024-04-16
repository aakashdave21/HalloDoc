using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Provider.Interfaces;
public interface IProviderDashboardService
{
   (List<RequestViewModel>, int totalCount) GetDashboardRequests(string status,string searchBy,int reqType,int pageNumber,int pageSize,int AspId);
    Dictionary<string,int> CountRequestByType(int AspId);

    void AcceptRequest(int ReqId);

}