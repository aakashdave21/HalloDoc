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
    public (List<RequestViewModel>,int totalCount) GetNewStatusRequest(string searchBy, int reqTypeId,int pageNumber,int pageSize)
    {

        var result = _dashboardRepo.GetNewRequest(searchBy, reqTypeId, pageNumber,pageSize);
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

        return (requestViewModels.ToList(),totalCount);
    }


    public List<RequestViewModel> GetPendingStatusRequest(string searchBy, int reqTypeId)
    {
        var requests = _dashboardRepo.GetPendingStatusRequest(searchBy, reqTypeId);

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

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetActiveStatusRequest(string searchBy, int reqTypeId)
    {
        var requests = _dashboardRepo.GetActiveStatusRequest(searchBy, reqTypeId);

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

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetConcludeStatusRequest(string searchBy, int reqTypeId)
    {
        var requests = _dashboardRepo.GetConcludeStatusRequest(searchBy, reqTypeId);

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

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetCloseStatusRequest(string searchBy, int reqTypeId)
    {
        var requests = _dashboardRepo.GetCloseStatusRequest(searchBy, reqTypeId);

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

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetUnpaidStatusRequest(string searchBy, int reqTypeId)
    {
        var requests = _dashboardRepo.GetUnpaidStatusRequest(searchBy, reqTypeId);

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

        return requestViewModels.ToList();
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
            Firstname = resData.Firstname,
            Lastname = resData.Lastname,
            Email = resData.Email,
            ConfirmationNumber = resData.Confirmationnumber ?? "No Confirmation Number",
            Phone = resData.Phonenumber,
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
        if(item.Transtoadmin == false){
            sentence = "admin transferred to physician "+ item.Transtophysicianid + "on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
        }else{
            sentence = "physician sent Transfer Request to admin "+ item.Adminid + " on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
        }
        sentences.Add(sentence);
    }

    ViewNotesViewModel viewNotes = new();
    if(reqData != null || patientNote != null || sentences.Count > 0){
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
    public void SaveAdditionalNotes(string AdditionalNote, int noteId,int reqId)
    {
        _dashboardRepo.SaveAdditionalNotes(AdditionalNote, noteId, reqId);
    }


}
