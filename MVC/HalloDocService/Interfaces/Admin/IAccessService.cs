using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IAccessService{
    
    AdminAccessViewModel GetAllAccessList();

    IEnumerable<MenuList> GetMenuList(int accountType);

    void DeleteAccess(int RoleId);

    void CreateNewRole(string roleName,int AccountType,List<int> MenuArray,int AspUserId);

    AdminAccessEditViewModel GetEditViewData(int Id);

    List<int> GetCheckedMenu(int Id);

    void UpdateRoleData(AdminAccessEditViewModel viewData,List<int> selectedMenusList, List<int> unSelectedMenusList);

    AdminAccountViewModel GetRoleAndState();

    void CreateAdminAccount(AdminAccountViewModel adminData);

    
}