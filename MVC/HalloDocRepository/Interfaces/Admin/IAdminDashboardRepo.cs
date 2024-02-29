using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocRepository.Admin.Interfaces;
public interface IAdminDashboardRepo
{
   (IEnumerable<Request> requests, int totalCount) GetNewRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   (IEnumerable<Request> requests, int totalCount) GetPendingStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   (IEnumerable<Request> requests, int totalCount) GetActiveStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   (IEnumerable<Request> requests, int totalCount) GetConcludeStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   (IEnumerable<Request> requests, int totalCount) GetCloseStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   (IEnumerable<Request> requests, int totalCount) GetUnpaidStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize);
   Dictionary<string,int> CountRequestByType();
   Request GetViewCaseDetails(int id);

   Requestnote GetViewNotesDetails(int reqId);
   Requestclient GetPatientNoteDetails(int reqId);
   IQueryable<Requeststatuslog> GetAllCancelNotes(int reqId);
   void SaveAdditionalNotes(string AdditionalNote,int noteId,int reqId);
   void ChangeStatusOfRequest(int reqId,short newStatus);
   void AddStatusLog(int reqId,short newStatus,short oldStatus,string reason,int? adminId,int? physicianId);
   short GetStatusOfRequest(int reqId);
   int? GetNoteIdFromRequestId(int reqId);

}
