using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocService.Admin.Implementation;
public class ProfileService : IProfileService
{

    private readonly IProfileRepo _profileRepo;

    public ProfileService(IProfileRepo profileRepo)
    {
        _profileRepo = profileRepo;
    }
    public AdminProfileViewModel GetAdminData(int AspUserId){
        var adminData = _profileRepo.GetAdminData(AspUserId);
        var RoleList = _profileRepo.GetAllRoles(1);
        var Regions = _profileRepo.GetServicedRegions(adminData.Id).Select(reg=> new RegionList(){
                Id = reg.Regionid,
                Name = reg.Region.Name
            }).ToList();
        var RegionsList = _profileRepo.GetAllRegions().Select(reg => new RegionList(){
                Id = reg.Id,
                Name = reg.Name
            }).ToList();

            var allRegionIds = RegionsList.Select(reg => reg.Id);
            var selectedRegionIds = Regions.Select(reg => reg.Id);
            var unselectedRegionIds = allRegionIds.Except(selectedRegionIds);
        
        AdminProfileViewModel adminProfile = new(){
            Id = adminData.Aspnetuserid,
            AdminId = adminData.Id,
            UserName = adminData.Aspnetuser.Username,
            Status = adminData.Status.HasValue ? int.Parse(adminData.Status.Value.ToString()) : 0,
            Role = adminData.Roleid,
            FirstName = adminData.Firstname,
            LastName = adminData.Lastname,
            Email = adminData.Email,
            ConfirmEmail = adminData.Email,
            Mobile = adminData.Mobile,
            Address1 = adminData.Address1,
            Address2 = adminData.Address2,
            City = adminData.City,
            State = adminData.Regionid,
            ZipCode = adminData.Zip,
            Phone = adminData.Altphone,
            roleSelect = RoleList.Select(role => new RoleList(){
                Id = role.Id,
                Name = role.Name
            }),
            regionSelect = Regions,
            AllRegionList = RegionsList,
            RegionUnSelect = RegionsList
            .Where(reg => unselectedRegionIds.Contains(reg.Id))
            .Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            })
        };
        return adminProfile;
    }

    public void UpdateAdminInfo(AdminProfileViewModel adminInfo){
        AdminTable adminDetailsUpdate = new(){
            Firstname = adminInfo.FirstName,
            Lastname = adminInfo.LastName,
            Email = adminInfo.Email,
            Mobile = adminInfo.Mobile
        };
        Aspnetuser aspUserEmailUpdate = new(){
            Email = adminInfo.Email,
        };
        _profileRepo.RemoveRegions(adminInfo.AdminId,adminInfo.UnCheckedRegions);
        _profileRepo.AddNewRegions(adminInfo.AdminId,adminInfo.CheckedRegions);
        _profileRepo.UpdateAdminInfo(adminDetailsUpdate,aspUserEmailUpdate,adminInfo.AdminId,adminInfo.Id);
    }
    public void UpdateBillingInfo(AdminProfileViewModel adminInfo){
        AdminTable adminDetailsUpdate = new(){
            Address1 = adminInfo.Address1,
            Address2 = adminInfo.Address2,
            Regionid = adminInfo.State,
            City = adminInfo.City,
            Altphone = adminInfo.Phone,
            Zip = adminInfo.ZipCode
        };
        _profileRepo.UpdateBillingInfo(adminDetailsUpdate,adminInfo.AdminId);
    }

    public string GetPassword(int aspUserId){
        return _profileRepo.GetPassword(aspUserId);
    }
    public void UpdatePassword(int aspUserId,string password){
        _profileRepo.UpdatePassword(aspUserId,password);
    }

}