using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Data.Common;

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

    public void RemoveRegions(int AdminId, List<int> UncheckedRegion){
        if(AdminId!=null && UncheckedRegion!=null){
            var UncheckedRegions = _dbContext.Adminregions.Where(adRegion => adRegion.Adminid == AdminId && UncheckedRegion.Contains(adRegion.Regionid));
            _dbContext.RemoveRange(UncheckedRegions);
           _dbContext.SaveChanges();
        }
    }
    public void AddNewRegions(int AdminId, List<int> UncheckedRegion){
        if(AdminId!=null && UncheckedRegion!=null){
            IEnumerable<Adminregion> adminRegionsToAdd = UncheckedRegion.Select(regionId => new Adminregion
            {
                Adminid = AdminId,
                Regionid = regionId,
                Updatedat = DateTime.Now 
            }).ToList();
            _dbContext.Adminregions.AddRange(adminRegionsToAdd);
            _dbContext.SaveChanges();
        }
    }

    public void UpdateBillingInfo(AdminTable adminInfo,int AdminId){
        if(AdminId!=null){
            AdminTable adminTableData = _dbContext.Admins.FirstOrDefault(user=>user.Id == AdminId);
            if(adminTableData!=null){
                adminTableData.Address1 = adminInfo.Address1;
                adminTableData.Address2 = adminInfo.Address2;
                adminTableData.City = adminInfo.City;
                adminTableData.Zip = adminInfo.Zip;
                adminTableData.Altphone = adminInfo.Altphone;
                adminTableData.Regionid = adminInfo.Regionid;

                _dbContext.SaveChanges();
                return;
            }
        }
        throw new Exception();
    }
    public string GetPassword(int aspUserId){
        if(aspUserId!=null){
            return _dbContext.Aspnetusers.FirstOrDefault(user => user.Id == aspUserId).Passwordhash;
        }
        throw new Exception();
    }

    public void UpdatePassword(int aspUserId,string password){
        if(aspUserId!=null){
            Aspnetuser aspUserDetails = _dbContext.Aspnetusers.FirstOrDefault(user => user.Id == aspUserId);
            if(aspUserDetails!=null){
                aspUserDetails.Passwordhash = password;
                _dbContext.SaveChanges();
                return;
            }
        }
        throw new Exception();
    }

    public IEnumerable<Region> GetAllRegions(){
        return _dbContext.Regions;
    }
}