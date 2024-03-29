using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IAccessRepo
{ 
    IEnumerable<Role> GetAllAccessList();

    void DeleteAccess(int? RoleId);

    IEnumerable<Menu> GetMenuList(int AccountType);

    void CreateNewRole(string roleName,int AccountType,List<int> MenuArray,int AspUserId);

    Role GetEditViewData(int Id);

    List<Rolemenu> GetCheckedMenu(int Id);

    void UpdateSameRoleData(int Id, string? RoleData ,List<int> selectedMenusList, List<int> unSelectedMenusList);

    void UpdateRoleData(int Id,string Role,short AccountType,List<int> selectedMenusList);

    bool CheckEmailExists(string? Email);

    void CreateNewAdmin(Aspnetuser? newAspUser,AdminTable? newAdmin ,Aspnetuserrole? newAspRole);
    void AddAdminRegion(List<Adminregion> adminRegionList);

}