using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;

namespace HalloDocService.Admin.Implementation;
public class AdminDashboardService : IAdminDashboardService
{

    private readonly IAdminDashboardRepo _dashboardRepo;
    public AdminDashboardService(IAdminDashboardRepo dashboardRepo){
        _dashboardRepo = dashboardRepo;
    }
    // Patient Request Implementation
    public List<RequestViewModel> GetNewStatusRequest(string searchBy,int reqTypeId){


        var requests = _dashboardRepo.GetNewRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid
            // Add other properties as needed
        });

        return requestViewModels.ToList();
    }
    
    
    public List<RequestViewModel> GetPendingStatusRequest(string searchBy,int reqTypeId){
        var requests = _dashboardRepo.GetPendingStatusRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician!=null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
            // Add other properties as needed
        });

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetActiveStatusRequest(string searchBy,int reqTypeId){
        var requests = _dashboardRepo.GetActiveStatusRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician!=null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
            // Add other properties as needed
        });

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetConcludeStatusRequest(string searchBy,int reqTypeId){
        var requests = _dashboardRepo.GetConcludeStatusRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician!=null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss")
            // Add other properties as needed
        });

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetCloseStatusRequest(string searchBy,int reqTypeId){
        var requests = _dashboardRepo.GetCloseStatusRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician!=null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-"
        });

        return requestViewModels.ToList();
    }
    public List<RequestViewModel> GetUnpaidStatusRequest(string searchBy,int reqTypeId){
        var requests = _dashboardRepo.GetUnpaidStatusRequest(searchBy,reqTypeId);

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " "+ r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName!=null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street +", "+ r.Requestclients.FirstOrDefault()?.City +", "+ r.Requestclients.FirstOrDefault()?.State +", "+ r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms!=null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianName = r.Physician!=null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-"
        });

        return requestViewModels.ToList();
    }
    public Dictionary<string,int> CountRequestByType()
    {
            return _dashboardRepo.CountRequestByType();
            
    }
    public ViewCaseViewModel GetViewCaseDetails(int id){
        Request resData = _dashboardRepo.GetViewCaseDetails(id);
        DateTime date = DateTime.ParseExact(resData.Requestclients.FirstOrDefault()?.Strmonth, "MMMM", CultureInfo.InvariantCulture);
        int year = resData.Requestclients.FirstOrDefault().Intyear ?? 0000;
        int day = resData.Requestclients.FirstOrDefault().Intdate ?? 1;
        date = new DateTime(year, date.Month, day);
        Console.WriteLine(date.ToString("yyyy-MM-dd"));
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

    public ViewNotesViewModel GetViewNotesDetails(int reqId){
        var reqData = _dashboardRepo.GetViewNotesDetails(reqId);
        ViewNotesViewModel viewNotes = new ViewNotesViewModel(){
            Id = reqData.Id,
            AdminNote = reqData.Adminnotes,
            NoteId = reqData.Id,
            PhysicianNote = reqData.Physiciannotes,
            ReqId = reqData.Requestid,
            AdditionalNote = reqData.Adminnotes
        };
        return viewNotes;
    }
        public void SaveAdditionalNotes(string AdditionalNote,int noteId){
            _dashboardRepo.SaveAdditionalNotes(AdditionalNote,noteId);
        }

   
}
