using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IProfileService{
    
    AdminProfileViewModel GetAdminData(int AspUserId);

    void UpdateAdminInfo(AdminProfileViewModel adminInfo);

    void UpdateBillingInfo(AdminProfileViewModel adminInfo);

    string GetPassword(int aspUserId);

    void UpdatePassword(int aspUserId,string password);
}