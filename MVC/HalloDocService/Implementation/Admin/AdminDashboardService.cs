using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;

namespace HalloDocService.Admin.Implementation;
public class AdminDashboardService : IAdminDashboardService
{

    private readonly IAdminDashboardRepo _dashboardRepo;
    public AdminDashboardService(IAdminDashboardRepo dashboardRepo){
        _dashboardRepo = dashboardRepo;
    }
    // Patient Request Implementation
    public List<RequestViewModel> GetNewStatusRequest(){


        var requests = _dashboardRepo.GetNewRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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
    
    
    public List<RequestViewModel> GetPendingStatusRequest(){
        var requests = _dashboardRepo.GetPendingStatusRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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
    public List<RequestViewModel> GetActiveStatusRequest(){
        var requests = _dashboardRepo.GetActiveStatusRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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
    public List<RequestViewModel> GetConcludeStatusRequest(){
        var requests = _dashboardRepo.GetConcludeStatusRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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
    public List<RequestViewModel> GetCloseStatusRequest(){
        var requests = _dashboardRepo.GetCloseStatusRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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
    public List<RequestViewModel> GetUnpaidStatusRequest(){
        var requests = _dashboardRepo.GetUnpaidStatusRequest();

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
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


}
