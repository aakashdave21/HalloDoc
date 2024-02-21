using HalloDocRepository.DataModels;

namespace HalloDocRepository.Admin.Interfaces;
public interface IAdminDashboardRepo
{
   IEnumerable<Request> GetNewRequest();
   IEnumerable<Request> GetPendingStatusRequest();
   IEnumerable<Request> GetActiveStatusRequest();
   IEnumerable<Request> GetConcludeStatusRequest();
   IEnumerable<Request> GetCloseStatusRequest();
   IEnumerable<Request> GetUnpaidStatusRequest();
   Dictionary<string,int> CountRequestByType();
}
