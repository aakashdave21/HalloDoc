using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocRepository.Admin.Interfaces;
public interface IAdminDashboardRepo
{
   (IEnumerable<Request> requests, int totalCount) GetNewRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   (IEnumerable<Request> requests, int totalCount) GetPendingStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   (IEnumerable<Request> requests, int totalCount) GetActiveStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   (IEnumerable<Request> requests, int totalCount) GetConcludeStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   (IEnumerable<Request> requests, int totalCount) GetCloseStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   (IEnumerable<Request> requests, int totalCount) GetUnpaidStatusRequest(string searchBy,int reqTypeId,int pageNumber,int pageSize,int region);
   Dictionary<string,int> CountRequestByType();
   Request GetViewCaseDetails(int id);

   Requestnote GetViewNotesDetails(int reqId);
   Requestclient GetPatientNoteDetails(int reqId);
   IQueryable<Requeststatuslog> GetAllCancelNotes(int reqId);
   void SaveAdditionalNotes(string AdditionalNote,int noteId,int reqId);
   void ChangeStatusOfRequest(int reqId,short newStatus);
   void AddStatusLog(int reqId,short newStatus,short oldStatus,string reason,int? adminId,int? physicianId,int? transToPhyId);
   short GetStatusOfRequest(int reqId);
   int? GetNoteIdFromRequestId(int reqId);

   Task<IEnumerable<Region>> GetRegions();
   Task<IEnumerable<Casetag>> GetCaseTag();
   Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId);
   void AddPhysicianToRequest(int reqId,int transPhyId);

   Request GetSingleRequestDetails(int reqId);

   void SetBlockFieldRequest(int reqId);
   void AddBlockRequest(Blockrequest newBlockReq);
   Request GetSingleRequest(int reqId);

   void DeleteDocument(int docId);

   IEnumerable<Healthprofessionaltype> GetAllProfessions();
   IEnumerable<Healthprofessional> GetBusinessByProfession(int professionId);
   Healthprofessional GetBusinessDetails(int businessId);
   void AddOrderDetails(Orderdetail order);
   void StoreAcceptToken(int reqId,string token,DateTime expirationTime);

   void AgreementAccept(int reqId);
   void AgreementReject(int reqId);

   void EditPatientInfo(string Email,string Phone,int patientId,int requestId);

   Request GetEncounterDetails(int reqId);
   void SubmitEncounter(Encounterform encounterform);

   void AddCallType(short callType,int reqId);

   IEnumerable<Request> FetchAllRequest();
}
