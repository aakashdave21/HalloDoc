using HalloDocRepository.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocService.Provider.Interfaces;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Admin.Interfaces;

namespace HalloDocService.Provider.Implementation;
public class ProviderDashboardService : IProviderDashboardService
{
      private readonly IProviderDashboardRepo _providerDashboardRepo;
        private readonly IAdminDashboardRepo _dashboardRepo;


    public ProviderDashboardService(IProviderDashboardRepo providerDashboardRepo,IAdminDashboardRepo dashboardRepo)
    {
        _providerDashboardRepo = providerDashboardRepo;
        _dashboardRepo = dashboardRepo;
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
            Notes = r.Symptoms ?? (r.Requestclients.FirstOrDefault()?.Notes),
            RequestType = r.Requesttypeid,
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
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
    public void SetTransferCase(int ReqId,int PhysicianId, string Description){
        _dashboardRepo.ChangeStatusOfRequest(ReqId, 1);
        _dashboardRepo.AddPhysicianToRequest(ReqId, null);
        _dashboardRepo.AddStatusLog(ReqId,1,2,Description,null,PhysicianId,null,true);
    }
    public bool CheckEncounterFinalized(int ReqId){
        return _providerDashboardRepo.CheckEncounterFinalized(ReqId);
    }

    public void ConcludeCare(int reqId, string providerNote,int? PhysicianId){
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        _dashboardRepo.ChangeStatusOfRequest(reqId, 8);
        _dashboardRepo.AddStatusLog(reqId,8,oldStatus,providerNote,null,PhysicianId,null,false);
    }

    public void FinalizeForm(int EncId, int ReqId){
        _providerDashboardRepo.FinalizeForm(EncId,ReqId);
    }
    public Aspnetuser? SendProfileRequest(int? PhyId){
        return _providerDashboardRepo.SendProfileRequest(PhyId);
    }
}