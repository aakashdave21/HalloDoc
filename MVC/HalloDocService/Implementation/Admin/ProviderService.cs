using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocService.Admin.Implementation;
public class ProviderService : IProviderService
{

    private readonly IProfileRepo _profileRepo;
    private readonly IProviderRepo _providerRepo;
    public ProviderService(IProfileRepo profileRepo, IProviderRepo providerRepo)
    {
        _profileRepo = profileRepo;
        _providerRepo = providerRepo;
    }

    public AdminProviderViewModel GetAllProviderData(string? regionId=null,string? order=null)
    {
        bool isAscending = order == null || order != "desc";
        AdminProviderViewModel providerViewModel = new()
        {
            AllRegionList = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }),
            AllPhysicianList = _providerRepo.GetAllPhysician(isAscending,regionId).Select(phy => new PhysicianList()
            {
                Id = phy.Id,
                IsNotificationStopped = phy.IsNotificationStop ?? false,
                // IsNotificationStopped = true,
                PhysicianName = phy.Firstname + " " + phy.Lastname,
                Role = phy.Role.Name,
                OnCallStatus = GetOnCallStatus(phy.OnCallStatus??0),
                Status = GetStatus(phy.Status ?? 0),
                RoleId = phy.Roleid ?? 0
            })
        };
        return providerViewModel;
    }
    private static string GetOnCallStatus(short statusId=0){
        return statusId switch
        {
            1 => "UnAvailable",
            2 => "OnCall",
            3 => "Busy",
            _ => "UnAvailable",
        };
    }
    private static string GetStatus(short statusId=0){
        return statusId switch
        {
            1 => "Pending",
            2 => "Active",
            3 => "NotActive",
            _ => "Pending",
        };
    }

    public void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds){
        _providerRepo.UpdateNotification(stopNotificationIds,startNotificationIds);
    }

    public Physician GetSingleProviderData(int Id){
        return _providerRepo.GetAllPhysician().FirstOrDefault(phy => phy.Id == Id);
    }
    
}
