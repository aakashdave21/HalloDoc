using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IProfileRepo
{
    AdminTable GetAdminData(int AspUserId);

    IEnumerable<Role> GetAllRoles(int AccountType);

    IEnumerable<Adminregion> GetServicedRegions(int AdminId);
    IEnumerable<Physicianregion> GetPhysicianServicedRegion(int PhyId);

    void UpdateAdminInfo(AdminTable adminInfo,Aspnetuser aspUserDetails,int AdminId,int AspUserId);

    void RemoveRegions(int AdminId, List<int> UncheckedRegion);
    void AddNewRegions(int AdminId, List<int> UncheckedRegion);

    void UpdateBillingInfo(AdminTable adminInfo,int AdminId);

    string GetPassword(int aspUserId);

    void UpdatePassword(int aspUserId,string password);

    IEnumerable<Region> GetAllRegions();
    
}