using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Data.Common;
using System.Reflection;


namespace HalloDocRepository.Admin.Implementation;
public class ProviderRepo : IProviderRepo
{
    private readonly HalloDocContext _dbContext;
    public ProviderRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Physician> GetAllPhysician(bool order = true, string? regionId = null)
    {
        IQueryable<Physician> query = _dbContext.Physicians.Include(phy => phy.Role).Include(phy => phy.Aspnetuser).Where(phy => phy.Isdeleted != true);
        if (!string.IsNullOrEmpty(regionId))
        {
            query = query.Where(physician => physician.Regionid == int.Parse(regionId));
        }
        if (order)
        {
            return query.OrderBy(physician => physician.Firstname)
                        .ThenBy(physician => physician.Lastname)
                        .ToList();
        }
        else
        {
            return query.OrderByDescending(physician => physician.Firstname)
                        .ThenByDescending(physician => physician.Lastname)
                        .ToList();
        }
    }
    public void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds){
        //startnotificationId -> true
        // stopNotificationId -> false
        var stopIds = stopNotificationIds.Select(int.Parse).ToList();
        _dbContext.Physicians
            .Where(physician => stopIds.Contains(physician.Id))
            .ToList()
            .ForEach(physician => physician.IsNotificationStop = false);

        var startIds = startNotificationIds.Select(int.Parse).ToList();
        _dbContext.Physicians
            .Where(physician => startIds.Contains(physician.Id))
            .ToList()
            .ForEach(physician => physician.IsNotificationStop = true);

        _dbContext.SaveChanges();

    }
    public void UpdateProviderPassword(int Id,string Password){
        Physician? physicianData = _dbContext.Physicians.Include(user => user.Aspnetuser).FirstOrDefault(phy => phy.Id == Id);
        if(physicianData?.Aspnetuser != null){
            physicianData.Aspnetuser.Passwordhash = Password;
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception();
    }

    public void UpdatePersonalInformation(int Id,int? RoleId,int? StatusId){
        Physician? phyDetails = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == Id);
        if(phyDetails != null){
            phyDetails.Roleid = RoleId;
            phyDetails.Status = (short?)StatusId;
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception();
    }

    public void UpdateGeneralInformation(int Id, Physician physicianData)
    {
        Physician? phyDetails = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == Id);
        if (phyDetails != null)
        {
            phyDetails.Firstname = physicianData.Firstname;
            phyDetails.Lastname = physicianData.Lastname;
            phyDetails.Email = physicianData.Email;
            phyDetails.Mobile = physicianData.Mobile;
            phyDetails.Medicallicense = physicianData.Medicallicense;
            phyDetails.Npinumber = physicianData.Npinumber;
            phyDetails.Syncemailaddress = physicianData.Syncemailaddress;
            
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Physician with the given ID not found.");
    }
    public void UpdateBillingInfo(int Id, Physician physicianData)
    {
        Physician? phyDetails = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == Id);
        if (phyDetails != null)
        {
            phyDetails.Address1 = physicianData.Address1;
            phyDetails.Address2 = physicianData.Address2;
            phyDetails.City = physicianData.City;
            phyDetails.Regionid = physicianData.Regionid;
            phyDetails.Zip = physicianData.Zip;
            phyDetails.Altphone = physicianData.Altphone;
            
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Physician with the given ID not found.");
    }
    public void UpdateBusinessInformation(int Id, Physician physicianData)
    {
        Physician? phyDetails = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == Id);
        if (phyDetails != null)
        {
            phyDetails.Businessname = physicianData.Businessname;
            phyDetails.Businesswebsite = physicianData.Businesswebsite;
            phyDetails.Adminnotes = physicianData.Adminnotes;
            if(!string.IsNullOrEmpty(physicianData.Photo)){
                phyDetails.Photo = physicianData.Photo;
            }
            if(!string.IsNullOrEmpty(physicianData.Signature)){
                phyDetails.Signature = physicianData.Signature;
            }
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Physician with the given ID not found.");
    }

    public string? GetFilePath(int? Id){
        if(Id != null){
            return _dbContext.Physicians?.FirstOrDefault(phy => phy.Id == Id)?.Signature;
        }
        throw new Exception("Physician with the given ID not found.");
    }
    public string? GetPhotoFilePath(int? Id){
        if(Id != null){
            return _dbContext.Physicians?.FirstOrDefault(phy => phy.Id == Id)?.Photo;
        }
        throw new Exception("Physician with the given ID not found.");
    }
    public void UploadDocument(int Id,string FileId, string filePath){
        Physician? physicianData = _dbContext.Physicians.FirstOrDefault(phy=>phy.Id == Id);
        if(physicianData!=null){
            Physicianfile PhysiciansFile = _dbContext.Physicianfiles.FirstOrDefault(file => file.Physicianid == Id);
            if (PhysiciansFile == null)
            {
                PhysiciansFile = new Physicianfile
                {
                    Physicianid = Id
                };
                _dbContext.Physicianfiles.Add(PhysiciansFile);
            }
            switch(FileId)
            {
                case "doc-1" : PhysiciansFile.Ica = filePath;
                                physicianData.Isagreementdoc = true;
                                break;
                case "doc-2" : PhysiciansFile.Backgroundcheck = filePath;
                                physicianData.Isbackgrounddoc = true;
                                break;
                case "doc-3" : PhysiciansFile.Hipaa = filePath;
                                physicianData.Istrainingdoc = true;
                                break;
                case "doc-4" : PhysiciansFile.Nda = filePath;
                                physicianData.Isnondisclosuredoc = true;
                                break;
                case "doc-5" : PhysiciansFile.License = filePath;
                                physicianData.Islicensedoc = true;
                                break;
                default : throw new Exception();
            }
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Physician not found.");
    }

    public string? GetAgreementFile(int Id,string FileId){
        Physicianfile? PhysiciansFile = _dbContext.Physicianfiles.FirstOrDefault(file => file.Physicianid == Id);
        return FileId switch
        {
            "doc-1" => PhysiciansFile?.Ica,
            "doc-2" => PhysiciansFile?.Backgroundcheck,
            "doc-3" => PhysiciansFile?.Hipaa,
            "doc-4" => PhysiciansFile?.Nda,
            "doc-5" => PhysiciansFile?.License,
            _ => throw new Exception(),
        };
    }

    public Physicianfile? PhysicianFileData(int Id){
        return _dbContext.Physicianfiles?.FirstOrDefault(file => file.Physicianid == Id);
    }

    public void DeleteProvider(int Id){
        Physician? physicianRecords = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == Id);
        if(physicianRecords != null){
            physicianRecords.Isdeleted = true;
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Physician not found");
    }

    public IEnumerable<Physicianlocation> GetAllProviderLocation(){
        return _dbContext.Physicianlocations.Include(loc => loc.Physician);
    }

    public void CreatePhysician(Physician physicianData){
        _dbContext.Physicians.Add(physicianData);
        _dbContext.SaveChanges();
    }

    public void AddPhysicianRegion(List<Physicianregion> physicianregions){
        _dbContext.Physicianregions.AddRange(physicianregions);
        _dbContext.SaveChanges();
    }

    public void AddPhysicianFile(Physicianfile physicianfile){
        _dbContext.Physicianfiles.Add(physicianfile);
        _dbContext.SaveChanges();
    }



}