using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;

namespace HalloDocService.Admin.Implementation;
public class AdminDashboardService : IAdminDashboardService
{

    private readonly IAdminDashboardRepo _dashboardRepo;
    public AdminDashboardService(IAdminDashboardRepo dashboardRepo)
    {
        _dashboardRepo = dashboardRepo;
    }
    // Patient Request Implementation
    public (List<RequestViewModel>, int totalCount) GetNewStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {

        var result = _dashboardRepo.GetNewRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
        });

        return (requestViewModels.ToList(), totalCount);
    }


   public (List<RequestViewModel>, int totalCount) GetPendingStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {
        var result = _dashboardRepo.GetPendingStatusRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
        });

         return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetActiveStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {
        var result = _dashboardRepo.GetActiveStatusRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
            // Add other properties as needed
        });

         return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetConcludeStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {
        var result = _dashboardRepo.GetConcludeStatusRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
            // Add other properties as needed
        });

         return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetCloseStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {
        var result = _dashboardRepo.GetCloseStatusRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-"
        });

         return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetUnpaidStatusRequest(string searchBy, int reqTypeId, int pageNumber, int pageSize)
    {
     
       var result = _dashboardRepo.GetUnpaidStatusRequest(searchBy, reqTypeId, pageNumber, pageSize);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-"
        });

         return (requestViewModels.ToList(), totalCount);
    }
    public Dictionary<string, int> CountRequestByType()
    {
        return _dashboardRepo.CountRequestByType();

    }
    public ViewCaseViewModel GetViewCaseDetails(int id)
    {
        Request resData = _dashboardRepo.GetViewCaseDetails(id);
        DateTime date = DateTime.ParseExact(resData.Requestclients.FirstOrDefault()?.Strmonth, "MMMM", CultureInfo.InvariantCulture);
        int year = resData.Requestclients.FirstOrDefault().Intyear ?? 0000;
        int day = resData.Requestclients.FirstOrDefault().Intdate ?? 1;
        date = new DateTime(year, date.Month, day);
        ViewCaseViewModel viewCase = new()
        {
            Id = resData.Id,
            Firstname = resData.Requestclients?.FirstOrDefault()?.Firstname,
            Lastname = resData.Requestclients?.FirstOrDefault()?.Lastname,
            Email = resData.Requestclients?.FirstOrDefault()?.Email,
            ConfirmationNumber = resData.Confirmationnumber ?? "No Confirmation Number",
            Phone = resData.Requestclients?.FirstOrDefault()?.Phonenumber,
            PropertyName = resData.PropertyName,
            Room = resData.Roomnoofpatient,
            DateOfBirth = date.ToString("yyyy-MM-dd"),
            Region = resData.Requestclients.FirstOrDefault()?.Region?.Name ?? " ",
            Symptoms = resData.Symptoms ?? resData.Requestclients?.FirstOrDefault()?.Notes,
            RequestType = resData?.Requesttype?.Name
        };

        return viewCase;
    }

    public ViewNotesViewModel GetViewNotesDetails(int reqId)
    {
        var reqData = _dashboardRepo.GetViewNotesDetails(reqId);
        var patientNote = _dashboardRepo.GetPatientNoteDetails(reqId);
        IQueryable<Requeststatuslog> cancelAndTransferNote = _dashboardRepo.GetAllCancelNotes(reqId);
        var PatientCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 7)?.Notes;
        var AdminCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 3 && req.Adminid != null && req.Physicianid == null)?.Notes;
        var PhysicianCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 3 && req.Adminid == null && req.Physicianid != null)?.Notes;
        var filteredNotes = cancelAndTransferNote.Where(req => req.Status == 2);
        var sentences = new List<string>();
        foreach (var item in filteredNotes)
        {
            string sentence;
            if (item.Transtoadmin == false)
            {
                sentence = "admin transferred to physician " + item.Transtophysicianid + "on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
            }
            else
            {
                sentence = "physician sent Transfer Request to admin " + item.Adminid + " on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
            }
            sentences.Add(sentence);
        }

        ViewNotesViewModel viewNotes = new();
        if (reqData != null || patientNote != null || sentences.Count > 0)
        {
            viewNotes = new ViewNotesViewModel
            {
                Id = reqData?.Id ?? 0,
                NoteId = reqData?.Id ?? 0,
                ReqId = reqId,
                AdminNote = reqData?.Adminnotes ?? "",
                PhysicianNote = reqData?.Physiciannotes ?? "",
                AdditionalNote = reqData?.Adminnotes ?? "",
                PatientNote = patientNote?.Notes ?? "",
                PatientCancelNote = PatientCancelNoteFromQuery ?? "",
                AdminCancelNote = AdminCancelNoteFromQuery ?? "",
                PhysicianCancelNote = PhysicianCancelNoteFromQuery ?? "",
                TransferNote = sentences.Any() ? sentences : new List<string>()
            };
        }

        return viewNotes;
    }
    public void SaveAdditionalNotes(string AdditionalNote, int noteId, int reqId)
    {
        _dashboardRepo.SaveAdditionalNotes(AdditionalNote, noteId, reqId);
    }

    public void CancleRequestCase(int reqId,string reason,string additionalNotes){
        short newStatus = 3;
        int adminId = 1;
        Nullable<int> physicianId = null;
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        int? noteId = _dashboardRepo.GetNoteIdFromRequestId(reqId);

        _dashboardRepo.ChangeStatusOfRequest(reqId,newStatus);
        _dashboardRepo.AddStatusLog(reqId,newStatus,oldStatus,reason,adminId,physicianId,null);

        if(noteId != null) _dashboardRepo.SaveAdditionalNotes(additionalNotes, (int)noteId, reqId);
        else _dashboardRepo.SaveAdditionalNotes(additionalNotes, 0, reqId);
        
    }

    public async Task<IEnumerable<Region>> GetRegions(){
        return await _dashboardRepo.GetRegions();
    }
    public async Task<IEnumerable<Casetag>> GetCaseTag(){
        return await _dashboardRepo.GetCaseTag();
    }

    public async Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId){
        return await _dashboardRepo.GetPhysicianByRegion(regionId);
    }

    public async Task AssignRequestCase(int reqId,int transPhyId,int? adminId,string desc){
        short newStatus = 2;
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        _dashboardRepo.ChangeStatusOfRequest(reqId,newStatus);
        _dashboardRepo.AddStatusLog(reqId,newStatus,oldStatus,desc,adminId,null,transPhyId);
        _dashboardRepo.AddPhysicianToRequest(reqId,transPhyId);
    }

    public async Task BlockRequestCase(int reqId,int? adminId,string reason){
        Request requestData = _dashboardRepo.GetSingleRequestDetails(reqId);
        short status = 11; //Blocked
        _dashboardRepo.ChangeStatusOfRequest(reqId,status);
        _dashboardRepo.SetBlockFieldRequest(reqId);

        Blockrequest newBlockRequest = new(){
            Requestid = reqId,
            Phonenumber = requestData.Requestclients.FirstOrDefault()?.Phonenumber,
            Email = requestData.Requestclients.FirstOrDefault()?.Email,
            Isactive = requestData.Status == 4 || requestData.Status == 5,
            Reason = reason
        };
        _dashboardRepo.AddBlockRequest(newBlockRequest);
    }
}
