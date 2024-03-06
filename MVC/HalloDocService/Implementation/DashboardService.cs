using HalloDocRepository.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Implementation;
public class DashboardService : IDashboardService
{

    private readonly IDashboardRepo _dashboardRepo;
    private readonly IPatientRequestRepo _patientRequestRepo;

    // Patient Request Implementation
    public DashboardService(IDashboardRepo dashboardRepo,IPatientRequestRepo patientRequestRepo)
    {
        _dashboardRepo = dashboardRepo;
        _patientRequestRepo = patientRequestRepo;
    }

    public User GetUserByEmail(string email){
        return _dashboardRepo.GetUserByEmail(email);
    }
    public IEnumerable<Request> GetUserRequest(int userId){
        return _dashboardRepo.GetUserRequest(userId);
    }

    public User GetUserData(int userId){
        return _dashboardRepo.GetUserData(userId);
    }

    public string GetUserRequestType(int status){
        return status switch
        {
            1 => "Unassigned",
            2 => "Accepted",
            3 => "Cancelled",
            4 => "MDOnSite",
            5 => "MDEnRoute",
            6 => "Conclude",
            7 => "CancelledByPatient",
            8 => "Closed",
            9 => "Unpaid",
            10 => "Clear",
            11 => "Blocked",
            // 12 => "Clear",
            // 13 => "CancelledByProvider",
            // 14 => "CCUploadedByClient",
            // 15 => "CCApprovedByAdmin",
            _ => "Unknown",
        };
    }

    public void EditUserProfile(int id,UserProfileViewModel userData){
        User user = new User{
            Firstname = userData.Firstname,
            Lastname = userData.Lastname,
            Mobile = userData.Mobile,
            Email = userData.Email,
            Birthdate = DateOnly.Parse(userData.Birthdate),
            Street = userData.Street,
            Zipcode = userData.Zipcode,
            City = userData.City,
            State = userData.State
        };
        _dashboardRepo.EditUserProfile(id,user);
    }

    public IEnumerable<Requestwisefile> GetAllRequestedDocuments (int reqId){
        return _dashboardRepo.GetAllRequestedDocuments(reqId);
    }

    public void UploadFileFromDocument(string filename,int reqId , int? AdminId=null){
        Requestwisefile newFile = new Requestwisefile{
            Filename = filename,
            Requestid = reqId,
            Adminid = AdminId,
            Createddate = DateTime.Now
        };
        _patientRequestRepo.AddDocumentDetails(newFile);
    }

}
