using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Implementation;
public class ProfileRepo : IProfileRepo
{
    private readonly HalloDocContext _dbContext;
    public ProfileRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public AdminTable GetAdminData(int AspUserId){
        return _dbContext.Admins.Include(user => user.Aspnetuser).FirstOrDefault(user=>user.Aspnetuserid == AspUserId);
    }

    public IEnumerable<Role> GetAllRoles(int AccountType=-1){
        if(AccountType == -1){
            return _dbContext.Roles;
        }else{
            return _dbContext.Roles.Where(role => role.Accounttype == AccountType);
        }
    }

    public IEnumerable<Adminregion> GetServicedRegions(int AdminId){
        return _dbContext.Adminregions.Include(reg => reg.Region).Where(reg => reg.Adminid == AdminId);
    }

    public void UpdateAdminInfo(AdminTable adminInfo,Aspnetuser aspUserDetails,int AdminId,int AspUserId){
        if(AdminId!=null && AspUserId!=null){
            AdminTable adminData = _dbContext.Admins.FirstOrDefault(admin => admin.Id == AdminId);
            Aspnetuser adminUserData = _dbContext.Aspnetusers.FirstOrDefault(user => user.Id == AspUserId);
            if(adminData!=null && adminUserData!=null){
                adminData.Firstname = adminInfo.Firstname;
                adminData.Lastname = adminInfo.Lastname;
                adminData.Email = adminInfo.Email;
                adminData.Mobile = adminInfo.Mobile;
                adminUserData.Email = adminInfo.Email;

                _dbContext.SaveChanges();
                return;
            }
            throw new Exception();
        }else{
            throw new Exception();
        }
    }
}