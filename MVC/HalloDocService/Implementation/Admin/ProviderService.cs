using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocService.Admin.Implementation;
public class ProviderService : IProviderService
{

    private readonly IProfileRepo _profileRepo;
    private readonly IProviderRepo _providerRepo;
    public ProviderService(IProfileRepo profileRepo, IProviderRepo providerRepo)
    {
        _profileRepo = profileRepo;
        _providerRepo = providerRepo;
    }

    public AdminProviderViewModel GetAllProviderData(string? regionId=null,string? order=null)
    {
        bool isAscending = order == null || order != "desc";
        AdminProviderViewModel providerViewModel = new()
        {
            AllRegionList = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }),
            AllPhysicianList = _providerRepo.GetAllPhysician(isAscending,regionId).Select(phy => new PhysicianList()
            {
                Id = phy.Id,
                IsNotificationStopped = phy.IsNotificationStop ?? false,
                // IsNotificationStopped = true,
                PhysicianName = phy.Firstname + " " + phy.Lastname,
                Role = phy.Role.Name,
                OnCallStatus = GetOnCallStatus(phy.OnCallStatus??0),
                Status = GetStatus(phy.Status ?? 0),
                RoleId = phy.Roleid ?? 0
            })
        };
        return providerViewModel;
    }
    private static string GetOnCallStatus(short statusId=0){
        return statusId switch
        {
            1 => "UnAvailable",
            2 => "OnCall",
            3 => "Busy",
            _ => "UnAvailable",
        };
    }
    private static string GetStatus(short statusId=0){
        return statusId switch
        {
            1 => "Pending",
            2 => "Active",
            3 => "NotActive",
            _ => "Pending",
        };
    }

    public void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds){
        _providerRepo.UpdateNotification(stopNotificationIds,startNotificationIds);
    }

    public Physician GetSingleProviderData(int Id){
        return _providerRepo.GetAllPhysician().FirstOrDefault(phy => phy.Id == Id);
    }

    public AdminPhysicianEditViewModel GetPhyisicianData(int Id){
        Physician physicianData = _providerRepo.GetAllPhysician().FirstOrDefault(phy => phy.Id == Id);
        Physicianfile? fileData = _providerRepo.PhysicianFileData(Id);
        AdminPhysicianEditViewModel phyEditView = new(){
            Id = physicianData.Id,
            Username = physicianData.Aspnetuser?.Username ?? "",
            StatusId = physicianData.Status,
            RoleId = physicianData.Roleid,
            Firstname = physicianData.Firstname,
            Lastname = physicianData.Lastname,
            Email = physicianData.Email,
            Phone = physicianData.Mobile,
            MedicalLicense = physicianData.Medicallicense,
            NPI = physicianData.Npinumber,
            SyncEmail = physicianData.Syncemailaddress,
            Address1 = physicianData.Address1,
            Address2 = physicianData.Address2,
            City = physicianData.City,
            State = physicianData.Regionid,
            Zipcode = physicianData.Zip,
            AltPhone = physicianData.Altphone,
            Businessname = physicianData.Businessname,
            BusinessWebsite = physicianData.Businesswebsite,
            AdminNote = physicianData.Adminnotes,
            UploadPhoto = physicianData.Photo,
            UploadSign = physicianData.Signature,
            AllRoleList = _profileRepo.GetAllRoles(-1).Select(role => new RoleList(){
                Id = role.Id,
                Name = role.Name
            }),
            AllRegionList = _profileRepo.GetAllRegions().Select(reg => new RegionList(){
                Id = reg.Id,
                Name = reg.Name
            }),
            IsICA = physicianData.Isagreementdoc ?? false,
            IsBgCheck = physicianData.Isbackgrounddoc ?? false,
            IsHIPAA = physicianData.Istrainingdoc ?? false,
            IsNDA = physicianData.Isnondisclosuredoc ?? false,
            IsLicenseDoc = physicianData.Islicensedoc ?? false,
            IsICAFile = fileData!=null ? "uploads/" + Path.GetFileName(fileData.Ica) : "",
            IsBgCheckFile = fileData!=null  ? "uploads/" + Path.GetFileName(fileData.Backgroundcheck) : "",
            IsHIPAAFile = fileData!=null  ? "uploads/" + Path.GetFileName(fileData.Hipaa) : "",
            IsNDAFile = fileData!=null  ? "uploads/" + Path.GetFileName(fileData.Nda) : "",
            IsLicenseDocFile = fileData!=null  ? "uploads/" + Path.GetFileName(fileData.License) : ""
        };
        return phyEditView;
    }

    public void UpdateProviderPassword(int Id,string Password){
        _providerRepo.UpdateProviderPassword(Id,Password);
    }

    public void UpdatePersonalInformation(AdminPhysicianEditViewModel viewData){
       _providerRepo.UpdatePersonalInformation(viewData.Id,viewData.RoleId,viewData.StatusId);
    }
    public void UpdateGeneralInformation(AdminPhysicianEditViewModel viewData){
        Physician physicianData = new(){
            Id = viewData.Id,
            Firstname = viewData.Firstname,
            Lastname = viewData.Lastname,
            Email = viewData.Email,
            Mobile = viewData.Phone,
            Medicallicense = viewData.MedicalLicense,
            Npinumber = viewData.NPI,
            Syncemailaddress = viewData.SyncEmail
        };
       _providerRepo.UpdateGeneralInformation(viewData.Id , physicianData);
    }

    public void UpdateBillingInfo(AdminPhysicianEditViewModel viewData){
        Physician physicianData = new(){
            Id = viewData.Id,
            Address1 = viewData.Address1,
            Address2 = viewData.Address2,
            City = viewData.City,
            Regionid = viewData.State,
            Zip = viewData.Zipcode,
            Altphone = viewData.AltPhone,
        };
        _providerRepo.UpdateBillingInfo(viewData.Id , physicianData);
    }
    public void UpdateBusinessInformation(AdminPhysicianEditViewModel viewData){
        Physician physicianData = new(){
            Id = viewData.Id,
            Businessname = viewData.Businessname,
            Businesswebsite = viewData.BusinessWebsite,
            Adminnotes = viewData.AdminNote
        };
        if(!string.IsNullOrEmpty(viewData.UploadPhoto)){
            physicianData.Photo = viewData.UploadPhoto;
        }
        if(!string.IsNullOrEmpty(viewData.UploadSign)){
            physicianData.Signature = viewData.UploadSign;
        }
        _providerRepo.UpdateBusinessInformation(viewData.Id , physicianData);
    }

    public string? GetFilesPath(int Id){
        return _providerRepo.GetFilePath(Id);
    }
    public string? GetPhotoFilePath(int Id){
        return _providerRepo.GetPhotoFilePath(Id);
    }

    public void UploadDocument(int Id,string FileId, string filePath){
        _providerRepo.UploadDocument(Id,FileId,filePath);
    }

    public string GetAgreementFile(int Id,string FileId){
        return _providerRepo.GetAgreementFile(Id,FileId);
    }

    public void DeleteProvider(int Id){
        _providerRepo.DeleteProvider(Id);
    }

    public AdminPhysicianCreateViewModel GetRoleAndState(){
        AdminPhysicianCreateViewModel adminView = new(){
            AllRoleList = _profileRepo.GetAllRoles(2).Select(role => new RoleList(){
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
        return adminView;
    }
    
}
