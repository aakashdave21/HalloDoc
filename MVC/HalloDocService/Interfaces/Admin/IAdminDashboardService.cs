
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IAdminDashboardService
{
    // Task<Action> GetDefaultRequestData();
    (List<RequestViewModel>,int totalCount) GetNewStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    (List<RequestViewModel>,int totalCount) GetPendingStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    (List<RequestViewModel>,int totalCount) GetActiveStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    (List<RequestViewModel>,int totalCount) GetConcludeStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    (List<RequestViewModel>,int totalCount) GetCloseStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    (List<RequestViewModel>,int totalCount) GetUnpaidStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
    Dictionary<string,int> CountRequestByType();
    ViewCaseViewModel GetViewCaseDetails(int id);
    ViewNotesViewModel GetViewNotesDetails(int reqId);
    void SaveAdditionalNotes(string AdditionalNote,int noteId,int reqId);
    void CancleRequestCase(int reqId,string reason,string additionalNotes);
}
