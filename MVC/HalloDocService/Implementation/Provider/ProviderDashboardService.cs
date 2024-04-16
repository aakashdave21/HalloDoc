using HalloDocRepository.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocService.Provider.Interfaces;
using HalloDocRepository.Provider.Interfaces;

namespace HalloDocService.Provider.Implementation;
public class ProviderDashboardService : IProviderDashboardService
{
      private readonly IProviderDashboardRepo _providerDashboardRepo;

    public ProviderDashboardService(IProviderDashboardRepo providerDashboardRepo)
    {
        _providerDashboardRepo = providerDashboardRepo;
    }
    public (List<RequestViewModel>, int totalCount) GetDashboardRequests(string status,string searchBy,int reqType,int pageNumber,int pageSize,int AspId){
       

        var result = _providerDashboardRepo.GetDashboardRequests(status , searchBy, reqType, pageNumber, pageSize, AspId);
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

    public Dictionary<string, int> CountRequestByType(int AspId)
    {
        return _providerDashboardRepo.CountRequestByType(AspId);
    }
    public void AcceptRequest(int ReqId){
        _providerDashboardRepo.AcceptRequest(ReqId);
    }
}