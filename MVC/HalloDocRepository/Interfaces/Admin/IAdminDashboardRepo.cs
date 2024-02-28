using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocRepository.Admin.Interfaces;
public interface IAdminDashboardRepo
{
   (IEnumerable<Request> requests, int totalCount) GetNewRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   IEnumerable<Request> GetPendingStatusRequest(string searchBy,int reqTypeId);
   IEnumerable<Request> GetActiveStatusRequest(string searchBy,int reqTypeId);
   IEnumerable<Request> GetConcludeStatusRequest(string searchBy,int reqTypeId);
   IEnumerable<Request> GetCloseStatusRequest(string searchBy,int reqTypeId);
   IEnumerable<Request> GetUnpaidStatusRequest(string searchBy,int reqTypeId);
   Dictionary<string,int> CountRequestByType();
   Request GetViewCaseDetails(int id);

   Requestnote GetViewNotesDetails(int reqId);
   Requestclient GetPatientNoteDetails(int reqId);
   IQueryable<Requeststatuslog> GetAllCancelNotes(int reqId);
   void SaveAdditionalNotes(string AdditionalNote,int noteId,int reqId);


}
