using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
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
        var Regions = _profileRepo.GetServicedRegions(adminData.Id);
        
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
            // State = adminData.,
            ZipCode = adminData.Zip,
            Phone = adminData.Mobile,
            roleSelect = RoleList.Select(role => new RoleList(){
                Id = role.Id,
                Name = role.Name
            }),
            regionSelect = Regions.Select(reg=> new RegionList(){
                Id = reg.Regionid,
                Name = reg.Region.Name
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
            Email = adminInfo.Email
        };
        _profileRepo.UpdateAdminInfo(adminDetailsUpdate,aspUserEmailUpdate,adminInfo.AdminId,adminInfo.Id);
    }
}