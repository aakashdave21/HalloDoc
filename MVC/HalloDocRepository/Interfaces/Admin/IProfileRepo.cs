using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IProfileRepo
{
    AdminTable GetAdminData(int AspUserId);

    IEnumerable<Role> GetAllRoles(int AccountType);

    IEnumerable<Adminregion> GetServicedRegions(int AdminId);

    void UpdateAdminInfo(AdminTable adminInfo,Aspnetuser aspUserDetails,int AdminId,int AspUserId);
    
}