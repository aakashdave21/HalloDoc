using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Implementation;
public class AccessRepo : IAccessRepo
{ 

    private readonly HalloDocContext _dbContext;

    public AccessRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Role> GetAllAccessList(){
        return _dbContext.Roles.Where(role => role.Isdeleted == false).OrderByDescending(role => role.Createdat).ThenBy(role => role.Updatedat);
    }

    public void DeleteAccess(int? RoleId){
        if(RoleId!=null){
            Role roleData = _dbContext.Roles.FirstOrDefault(role => role.Id == RoleId);
            if(roleData!=null){
                foreach (var physician in _dbContext.Physicians.Where(p => p.Roleid == RoleId))
                {
                    physician.Roleid = null;
                }
                foreach (var admin in _dbContext.Admins.Where(a => a.Roleid == RoleId))
                {
                    admin.Roleid = null;
                }
                roleData.Isdeleted = true;
                _dbContext.SaveChanges();
                return;
            }
        }
        throw new Exception();
        
    }

    public IEnumerable<Menu> GetMenuList(int AccountType){
        IQueryable<Menu> query = _dbContext.Menus;
        if(AccountType == 1 || AccountType == 2){
            query = query.Where(menu=>menu.Accounttype == AccountType);
            return query;
        }
        return query;
    }

    public void CreateNewRole(string roleName,int AccountType,List<int> MenuArray,int AspUserId){
        if(AccountType == 0){
            CreateNewRole(roleName,1,MenuArray,AspUserId);
            CreateNewRole(roleName,2,MenuArray,AspUserId);
            return;
        }
        Role newRole = new(){
            Name = roleName,
            Accounttype = (short)AccountType,
            CreatedBy = AspUserId,
            UpdatedBy = AspUserId
        };
        _dbContext.Roles.Add(newRole);
        _dbContext.SaveChanges();

        int newRoleId = newRole.Id;
        foreach (var item in MenuArray)
        {
            Rolemenu newRoleMenu = new(){
                Roleid = newRoleId,
                Menuid = item
            };
            _dbContext.Rolemenus.Add(newRoleMenu);
        }
         _dbContext.SaveChanges();
    }

    public Role GetEditViewData(int Id){
        return _dbContext.Roles.FirstOrDefault(role => role.Id == Id);
    }

    public List<Rolemenu> GetCheckedMenu(int Id){
        return _dbContext.Rolemenus.Include(role => role.Role).Where(rolemenu => rolemenu.Roleid == Id).ToList();
    }

    public void UpdateSameRoleData(int Id, string? RoleName ,List<int> selectedMenusList, List<int> unSelectedMenusList){
        Role? RoleDetails = _dbContext.Roles.FirstOrDefault(role => role.Id == Id);
        if(RoleDetails != null){
            RoleDetails.Name = RoleName;
            RoleDetails.Updatedat = DateTime.Now;
            _dbContext.SaveChanges();
        }

        var recordsToDelete = _dbContext.Rolemenus
                                .Where(rm => rm.Roleid == Id && unSelectedMenusList.Contains(rm.Menuid))
                                .ToList();
        if (recordsToDelete.Any())
        {
            _dbContext.Rolemenus.RemoveRange(recordsToDelete);
            _dbContext.SaveChanges();
        }

        List<Rolemenu> recordsToAdd = selectedMenusList.Select(rm => new Rolemenu(){
            Roleid = Id,
            Menuid = rm
        }).ToList();

        if (recordsToAdd.Any())
        {
            _dbContext.Rolemenus.AddRange(recordsToAdd);
            _dbContext.SaveChanges();
        }
    }

    public void UpdateRoleData(int Id,string Role,short AccountType,List<int> selectedMenusList){
        Role? RoleDetails = _dbContext.Roles.FirstOrDefault(role => role.Id == Id);
        if(RoleDetails != null){
            RoleDetails.Name = Role;
            RoleDetails.Accounttype = AccountType;
            RoleDetails.Updatedat = DateTime.Now;
            _dbContext.SaveChanges();
        }
        var recordsToDelete = _dbContext.Rolemenus
                                .Where(rm => rm.Roleid == Id)
                                .ToList();
        if (recordsToDelete.Any())
        {
            _dbContext.Rolemenus.RemoveRange(recordsToDelete);
            _dbContext.SaveChanges();
        }

        List<Rolemenu> recordsToAdd = selectedMenusList.Select(rm => new Rolemenu(){
            Roleid = Id,
            Menuid = rm
        }).ToList();

        if (recordsToAdd.Any())
        {
            _dbContext.Rolemenus.AddRange(recordsToAdd);
            _dbContext.SaveChanges();
        }
    }

    public bool CheckEmailExists(string? Email){
        if(Email != null){
            return _dbContext.Aspnetusers.Any(user => user.Email == Email);
        }
        throw new Exception();
    }

    public void CreateNewAdmin(Aspnetuser? newAspUser=null,AdminTable? newAdmin=null,Aspnetuserrole? newAspRole=null){
        if(newAspUser!=null){
            _dbContext.Aspnetusers.Add(newAspUser);
        }
        if(newAdmin!=null){
            _dbContext.Admins.Add(newAdmin);
        }
        if(newAspRole!=null){
            _dbContext.Aspnetuserroles.Add(newAspRole);
        }
        _dbContext.SaveChanges();
    }

    public void AddAdminRegion(List<Adminregion> adminRegionList){
        foreach (var item in adminRegionList)
        {
            Console.WriteLine(item.Adminid);
            Console.WriteLine(item.Regionid);
        }
        _dbContext.Adminregions.AddRange(adminRegionList);
        _dbContext.SaveChanges();
    }

    
}