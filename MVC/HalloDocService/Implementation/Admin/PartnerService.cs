using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;

namespace HalloDocService.Admin.Implementation;
public class PartnerService : IPartnerService
{

    private readonly IPartnerRepo _partnerRepo;
    private readonly IAdminDashboardRepo _dashboardRepo;
    private readonly IProfileRepo _profileRepo;
    public PartnerService(IPartnerRepo partnerRepo, IAdminDashboardRepo dashboardRepo, IProfileRepo profileRepo)
    {
        _partnerRepo = partnerRepo;
        _dashboardRepo = dashboardRepo;
        _profileRepo = profileRepo;
    }

    public PartnerViewModel GetVendorList(string vendorName = "", int ProfessionId = 0)
    {
        IEnumerable<Healthprofessional> VendorInfo = _partnerRepo.GetVendorList(vendorName, ProfessionId);
        PartnerViewModel vendorDetails = new()
        {
            AllProfessionsList = _dashboardRepo.GetAllProfessions().Select(prof => new ProfessionList()
            {
                ProfessionId = prof.Id,
                ProfessionName = prof.Professionname,
            }),

            VendorsDetails = VendorInfo.Select(vendor => new VendorDetail()
            {
                Id = vendor.Id,
                BusinessName = vendor.Vendorname,
                ProfessionName = vendor.ProfessionNavigation.Professionname,
                Email = vendor.Email,
                FaxNumber = vendor.Faxnumber,
                PhoneNumber = vendor.Phonenumber,
                BusinessContact = vendor.Businesscontact
            })
        };
        return vendorDetails;
    }

    public VendorDetail GetAllRegionAndProfession()
    {
        VendorDetail vendorDetail = new(){
            AllRegionsList = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }),
            AllProfessionsList = _dashboardRepo.GetAllProfessions().Select(prof => new ProfessionList()
            {
                ProfessionId = prof.Id,
                ProfessionName = prof.Professionname,
            }),
        };
        return vendorDetail;
    }

    public void AddVendor(VendorDetail vendorDetail,bool IsUpdate=false){
        Healthprofessional vendorInfo = new(){
            Vendorname = vendorDetail.BusinessName,
            Businesscontact = vendorDetail.BusinessContact,
            Email = vendorDetail.Email,
            Faxnumber = vendorDetail.FaxNumber,
            Phonenumber = vendorDetail.PhoneNumber,
            Address = vendorDetail.Street,
            City = vendorDetail.City,
            Regionid = vendorDetail.State,
            Zip = vendorDetail.ZipCode,
            Profession = vendorDetail.ProfessionId,
        };
        if(IsUpdate==true){
            vendorInfo.Id = vendorDetail.Id;
        }
        _partnerRepo.AddVendor(vendorInfo);
    }

    public void DeleteVendor(int Id){
        _partnerRepo.DeleteVendor(Id);
    }

    public VendorDetail GetSingleBusiness(int Id){
        Healthprofessional vendorDetails = _partnerRepo.GetSingleBusiness(Id);
        VendorDetail vendorInfo = new(){
             AllRegionsList = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }),
            AllProfessionsList = _dashboardRepo.GetAllProfessions().Select(prof => new ProfessionList()
            {
                ProfessionId = prof.Id,
                ProfessionName = prof.Professionname,
            }),
            BusinessName = vendorDetails.Vendorname,
            ProfessionId = vendorDetails.Profession,
            FaxNumber = vendorDetails.Faxnumber,
            PhoneNumber = vendorDetails.Phonenumber,
            Email = vendorDetails.Email,
            BusinessContact = vendorDetails.Businesscontact,
            Street = vendorDetails.Address,
            City = vendorDetails.City,
            State = vendorDetails.Regionid,
            ZipCode = vendorDetails.Zip,
        };
        return vendorInfo;
    }

}