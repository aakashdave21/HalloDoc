using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IPartnerService{
    PartnerViewModel GetVendorList(string vendorName = "", int ProfessionId = 0);
    VendorDetail GetAllRegionAndProfession();

    void AddVendor(VendorDetail vendorDetail,bool IsUpdate);
    void DeleteVendor(int Id);

    VendorDetail GetSingleBusiness(int Id);
}