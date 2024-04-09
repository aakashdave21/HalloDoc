using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IPartnerRepo
{
    IEnumerable<Healthprofessional> GetVendorList(string vendorName = "", int ProfessionId = 0);

    void AddVendor(Healthprofessional vendorInfo);

    void DeleteVendor(int Id);

    Healthprofessional GetSingleBusiness(int Id);
}