
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IAdminDashboardService
{
    IEnumerable<HeaderMenu>? GetRoleOfUser(int AspUserId);
    (List<RequestViewModel>,int totalCount) GetNewStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    (List<RequestViewModel>,int totalCount) GetPendingStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    (List<RequestViewModel>,int totalCount) GetActiveStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    (List<RequestViewModel>,int totalCount) GetConcludeStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    (List<RequestViewModel>,int totalCount) GetCloseStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    (List<RequestViewModel>,int totalCount) GetUnpaidStatusRequest(string? searchBy,int reqTypeId,int pageNumber,int pageSize, int region);
    Dictionary<string,int> CountRequestByType();
    ViewCaseViewModel GetViewCaseDetails(int id);
    ViewNotesViewModel GetViewNotesDetails(int reqId, int reqType = 1);
    void SaveAdditionalNotes(string? AdditionalNote,int noteId,int reqId, int reqType);
    void CancleRequestCase(int reqId,string reason,string additionalNotes);

    List<Region> GetRegions();
    Task<IEnumerable<Casetag>> GetCaseTag();
    Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId);

    void AssignRequestCase(int reqId,int phyId,int? adminId,string desc);
    Task BlockRequestCase(int reqId,int? adminId,string reason);

    Request GetSingleRequest(int reqId);
    void DeleteDocument(int docId);

    IEnumerable<ProfessionList> GetAllProfessions();

    IEnumerable<BusinessList> GetBusinessByProfession(int ProfessionId);
    SendOrderViewModel GetBusinessDetails(int businessId);

    void AddOrderDetails(SendOrderViewModel sendOrders);

    void SetClearCase(int RequestId);

    void SetTransferCase(int reqId,int oldphyId,int physician,string description);

    void StoreAcceptToken(int reqId,string token,DateTime expirationTime);

    void AgreementAccept(int reqId);
    void AgreementReject(int reqId,string reason);

    CloseCaseViewModel CloseCase(int RequestId);

    void EditPatientInfo(string Email,string Phone,int patientId,int requestId);

    void CloseCaseSubmit(int reqId);

    void ConsultEncounter(int reqId);

    EncounterFormViewModel GetEncounterDetails(int reqId);

    void HouseCallEncounter(int reqId,string status);

    void SubmitEncounter(EncounterFormViewModel encounterForm);

    IEnumerable<Request> FetchAllRequest();

    void CreateRequest(PatientRequestViewModel patientRequest,int AccountType = 1);
    }
