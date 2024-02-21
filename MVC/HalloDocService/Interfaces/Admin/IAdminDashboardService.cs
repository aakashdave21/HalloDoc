
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IAdminDashboardService
{
    // Task<Action> GetDefaultRequestData();
    List<RequestViewModel> GetNewStatusRequest();
    List<RequestViewModel> GetPendingStatusRequest();
    List<RequestViewModel> GetActiveStatusRequest();
    List<RequestViewModel> GetConcludeStatusRequest();
    List<RequestViewModel> GetCloseStatusRequest();
    List<RequestViewModel> GetUnpaidStatusRequest();
    Dictionary<string,int> CountRequestByType();
}
