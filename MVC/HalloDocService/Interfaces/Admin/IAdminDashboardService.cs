
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IAdminDashboardService
{
    // Task<Action> GetDefaultRequestData();
    List<RequestViewModel> GetNewStatusRequest(string searchBy,int reqTypeId);
    List<RequestViewModel> GetPendingStatusRequest(string searchBy,int reqTypeId);
    List<RequestViewModel> GetActiveStatusRequest(string searchBy,int reqTypeId);
    List<RequestViewModel> GetConcludeStatusRequest(string searchBy,int reqTypeId);
    List<RequestViewModel> GetCloseStatusRequest(string searchBy,int reqTypeId);
    List<RequestViewModel> GetUnpaidStatusRequest(string searchBy,int reqTypeId);
    Dictionary<string,int> CountRequestByType();
    ViewCaseViewModel GetViewCaseDetails(int id);
    ViewNotesViewModel GetViewNotesDetails(int reqId);
    void SaveAdditionalNotes(string AdditionalNote,int noteId);
}
