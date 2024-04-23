using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.Interfaces;

namespace HalloDocService.Admin.Implementation;
public class ProviderService : IProviderService
{

    private readonly IProfileRepo _profileRepo;
    private readonly IProviderRepo _providerRepo;
    private readonly IPatientLoginRepo _patientLoginRepo;
    private readonly IPatientRequestRepo _patientRequestRepo;
    public ProviderService(IProfileRepo profileRepo, IProviderRepo providerRepo,IPatientLoginRepo patientLoginRepo,IPatientRequestRepo patientRequestRepo)
    {
        _profileRepo = profileRepo;
        _providerRepo = providerRepo;
        _patientLoginRepo = patientLoginRepo;
        _patientRequestRepo = patientRequestRepo;

    }

    public AdminProviderViewModel GetAllProviderData(string? regionId=null,string? order=null,int PageNum = 1 ,int PageSize = 5)
    {
        bool isAscending = order == null || order != "desc";
        var (ProviderList, totalCount) = _providerRepo.GetAllPhysician(isAscending,regionId,PageNum,PageSize);
        int startIndex = (PageNum - 1) * PageSize + 1;
        AdminProviderViewModel providerViewModel = new()
        {
            AllRegionList = _profileRepo.GetAllRegions().Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            }),
            AllPhysicianList = ProviderList.Select(phy => new PhysicianList()
            {
                Id = phy.Id,
                IsNotificationStopped = phy.IsNotificationStop ?? false,
                PhysicianName = phy.Firstname + " " + phy.Lastname,
                Role = phy?.Role?.Name,
                OnCallStatus = _providerRepo.GetOnCallStatus(phy.Id),
                Status = GetStatus(phy.Status ?? 0),
                RoleId = phy.Roleid ?? 0
            }),
            TotalCount = totalCount,
            CurrentPage = PageNum,
            CurrentPageSize = PageSize,
            PageRangeStart = totalCount == 0 ? 0 : startIndex,
            PageRangeEnd = Math.Min(startIndex + PageSize - 1, totalCount),
            TotalPage = (int)Math.Ceiling((double)totalCount / PageSize)
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
        return _providerRepo.GetAllPhysicianList().FirstOrDefault(phy => phy.Id == Id);
    }

    public AdminPhysicianEditViewModel GetPhyisicianData(int Id){
        var Regions = _profileRepo.GetPhysicianServicedRegion(Id).Select(reg=> new RegionList(){
                Id = reg.Regionid ?? 0,
                Name = reg?.Region?.Name
            }).ToList();
        var RegionsList = _profileRepo.GetAllRegions().Select(reg => new RegionList(){
                Id = reg.Id,
                Name = reg.Name
            }).ToList();

            var allRegionIds = RegionsList.Select(reg => reg.Id);
            var selectedRegionIds = Regions.Select(reg => reg.Id);
            var unselectedRegionIds = allRegionIds.Except(selectedRegionIds);

        Physician physicianData = _providerRepo.GetAllPhysicianList().FirstOrDefault(phy => phy.Id == Id);
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
            IsLicenseDocFile = fileData!=null  ? "uploads/" + Path.GetFileName(fileData.License) : "",
            RegionSelect = Regions,
            RegionUnSelect = RegionsList
            .Where(reg => unselectedRegionIds.Contains(reg.Id))
            .Select(reg => new RegionList()
            {
                Id = reg.Id,
                Name = reg.Name
            })
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
    
    public ProviderLocationViewModel GetAllProviderLocation(){
        ProviderLocationViewModel locationList = new(){
            ProviderLocationList = _providerRepo.GetAllProviderLocation().Select(loc => new ProviderLocation(){
                Id = loc.Id,
                Latitude = loc.Latitude ?? 0,
                Longitude = loc.Longitude ?? 0,
                PhyicianName = loc?.Physician?.Firstname + " " + loc?.Physician?.Lastname,
                Address = loc?.Address
            })
        };
        return locationList;
    }

    public void CreatePhysician(AdminPhysicianCreateViewModel viewData){
        Aspnetuser alreadyExists = _patientLoginRepo.ValidateUser(viewData.Email);
        if(alreadyExists!=null){
            throw new Exception("Email already exists");
        }
        Aspnetuser aspnetuser= new(){
            Username = viewData.Username,
            Passwordhash = viewData.Password,
            Email = viewData.Email,
            Phonenumber = viewData.Phone
        };
        _patientRequestRepo.NewAspUserAdd(aspnetuser);
        Aspnetuserrole aspnetrole = new(){
            Userid = aspnetuser.Id,
            Roleid = 2
        };
        _patientRequestRepo.NewRoleAdded(aspnetrole);
        Physician physicianData = new(){
            Aspnetuserid = aspnetuser.Id,
            Firstname = viewData.Firstname ?? "",
            Lastname = viewData.Lastname,
            Email = viewData.Email ?? "",
            Mobile = viewData.Phone,
            Medicallicense = viewData.MedicalLicense,
            Npinumber = viewData.NPI,
            Address1 = viewData.Address1,
            Address2 = viewData.Address2,
            City = viewData.City,
            Regionid = viewData.State,
            Zip = viewData.Zipcode,
            Altphone = viewData.AltPhone,
            Businessname = viewData?.Businessname ?? "",
            Businesswebsite = viewData?.BusinessWebsite ?? "",
            Adminnotes = viewData?.AdminNote ?? "",
            Roleid = viewData?.RoleId,
            OnCallStatus = 1,
            Photo = viewData?.UploadPhoto ?? "",
            Isbackgrounddoc = viewData?.IsBgCheckFileName != null,
            Isnondisclosuredoc = viewData?.IsNDAFileName != null,
            Isagreementdoc = viewData?.IsICAFileName != null,
            Istrainingdoc = viewData?.IsHIPAAFileName != null,
            Islicensedoc = viewData?.IsLicenseDocFileName != null
        };
        _providerRepo.CreatePhysician(physicianData);

        List<Physicianregion> physicianregions = new();
        foreach(var item in viewData.AllCheckBoxRegionList){
            if(item.IsSelected == true){
                physicianregions.Add(new Physicianregion(){
                    Regionid = item.Id,
                    Physicianid = physicianData.Id
                });
            }
        }
        _providerRepo.AddPhysicianRegion(physicianregions);

        Physicianfile physicianfile = new(){
            Physicianid = physicianData.Id,
            Ica = viewData.IsICAFileName,
            Nda = viewData.IsNDAFileName,
            Backgroundcheck = viewData.IsBgCheckFileName,
            Hipaa = viewData.IsHIPAAFileName,
            License = viewData.IsLicenseDocFileName
        };
        _providerRepo.AddPhysicianFile(physicianfile);
    }
}
