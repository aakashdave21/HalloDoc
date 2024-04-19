using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocService.Admin.Implementation;
public class AccessService : IAccessService
{ 

    private readonly IAccessRepo _accessRepo;

    private readonly IProfileRepo _profileRepo;
    
    public AccessService(IAccessRepo accessRepo,IProfileRepo profileRepo)
    {
        _accessRepo = accessRepo;
        _profileRepo = profileRepo;
    }

    public AdminAccessViewModel GetAllAccessList(){
        AdminAccessViewModel adminAccessView = new(){
            AccessRoleList = _accessRepo.GetAllAccessList().Select(role => new AccessList(){
                Id = role.Id,
                RoleName = role.Name,
                AccountType = GetAccountType(role.Accounttype)
            })
        };
        return adminAccessView;
    }
    private static string GetAccountType(short? AccountId){
        return AccountId switch
        {
            1 => "Admin",
            2 => "Provider",
            3 => "Patient",
            _ => "Unknown",
        };
    }

    public void DeleteAccess(int RoleId){
        _accessRepo.DeleteAccess(RoleId);
    }

    public IEnumerable<MenuList> GetMenuList(int accountType){
            IEnumerable<MenuList> menulists = _accessRepo.GetMenuList(accountType).Select(menu => new MenuList(){
                Id = menu.Id,
                Name = menu.Name,
                AccountType = menu.Accounttype
            });
            return menulists;
    }
    public IEnumerable<HeaderMenu> GetAllMenuList(){
            IEnumerable<HeaderMenu> menulists = _accessRepo.GetMenuList(0).Select(menu => new HeaderMenu(){
                Id = menu.Id,
                Name = menu.Name,
                Title = menu.Title,
                SortOrder = menu.Sortorder,
                AccountType = menu.Accounttype
            });
            return menulists;
    }
    public void CreateNewRole(string roleName,int AccountType,List<int> MenuArray,int AspUserId){
        _accessRepo.CreateNewRole(roleName,AccountType,MenuArray,AspUserId);
    }

    public AdminAccessEditViewModel GetEditViewData(int Id){
        Role editRole = _accessRepo.GetEditViewData(Id);
        AdminAccessEditViewModel viewEditModel = new(){
            Id = Id,
            Role = editRole.Name,
            AccountType = editRole.Accounttype,
            AccessMenuList = GetMenuList((int)editRole.Accounttype),
            SelectedIds = GetCheckedMenu(Id)
        };
        return viewEditModel;
    }

    public List<int> GetCheckedMenu(int Id){
        return _accessRepo.GetCheckedMenu(Id).Select(role => role.Menuid).ToList();
    }

    public void UpdateRoleData(AdminAccessEditViewModel viewData,List<int> selectedMenusList, List<int> unSelectedMenusList){
        short? accountType = _accessRepo.GetEditViewData(viewData.Id).Accounttype;
        if(accountType == viewData.AccountType){
            _accessRepo.UpdateSameRoleData(viewData.Id , viewData.Role ,selectedMenusList,unSelectedMenusList);
        }else{
            _accessRepo.UpdateRoleData(viewData.Id,viewData.Role,(short)viewData.AccountType,selectedMenusList);
        }
    }

    public AdminAccountViewModel GetRoleAndState(){
        AdminAccountViewModel AccountData = new(){
            AllRoleList = _profileRepo.GetAllRoles(1).Select(role => new RoleList(){
                Id = role.Id,
                Name = role.Name
            }),
            AllRegionsList = _profileRepo.GetAllRegions().Select(reg => new AllRegionList(){
                Id = reg.Id,
                Name = reg.Name,
                IsSelected = false
            }).ToList(),
            AllCheckBoxRegionList =  _profileRepo.GetAllRegions().Select(reg => new AllRegionList(){
                Id = reg.Id,
                Name = reg.Name,
                IsSelected = false
            }).ToList(),
        };
        return AccountData;
    }
    
    public void CreateAdminAccount(AdminAccountViewModel adminData){
        bool isEmailExists = _accessRepo.CheckEmailExists(adminData.Email);
        if(isEmailExists==false){
            Aspnetuser newAspUser = new(){
                Email = adminData.Email,
                Passwordhash = adminData.Password,
                Emailconfirmed = true,
                Username = adminData.Username,
                Phonenumber = adminData.PhoneNumber
            };
            _accessRepo.CreateNewAdmin(newAspUser,null,null);

            AdminTable newAdmin = new(){
                Aspnetuserid = newAspUser.Id,
                Firstname = adminData.FirstName,
                Lastname = adminData.LastName,
                Email = adminData.Email,
                Mobile = adminData.PhoneNumber,
                Address1 = adminData.Address1,
                Address2 = adminData.Address2,
                City = adminData.City,
                Regionid = adminData.State,
                Roleid = adminData.RoleId,
                Zip = adminData.Zip,
                Altphone = adminData.AlternatePhone,
                Status = 1,
                Createdby = adminData.CreatedUser,
            };
            _accessRepo.CreateNewAdmin(null,newAdmin,null);

            Aspnetuserrole newAspRole = new(){
                Userid = newAspUser.Id,
                Roleid = 1          
            };
            _accessRepo.CreateNewAdmin(null,null,newAspRole);

            List<Adminregion> adminRegionList = new();
            foreach (var item in adminData.AllCheckBoxRegionList)
            {
                if (item.IsSelected)
                {
                    adminRegionList.Add(new Adminregion
                    {
                        Adminid = newAdmin.Id,
                        Regionid = item.Id
                    });
                }
            }
            _accessRepo.AddAdminRegion(adminRegionList);
            return;
        }
        throw new Exception("EmailAlreadyExists");
    }
    
}