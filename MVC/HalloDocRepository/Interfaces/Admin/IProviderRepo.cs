using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IProviderRepo
{ 
    IEnumerable<Physician> GetAllPhysician(bool order=true,string? regionId=null);
    void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds);

    void UpdateProviderPassword(int Id,string Password);

    void UpdatePersonalInformation(int Id,int? RoleId,int? StatusId);
    void UpdateGeneralInformation(int Id ,Physician physicianData);
    void UpdateBillingInfo(int Id ,Physician physicianData);
    void UpdateBusinessInformation(int Id ,Physician physicianData);
    string? GetFilePath(int? Id);
    string? GetPhotoFilePath(int? Id);
    void UploadDocument(int Id,string FileId, string filePath);
    string? GetAgreementFile(int Id,string FileId);

    Physicianfile? PhysicianFileData(int Id);

    void DeleteProvider(int Id);

}