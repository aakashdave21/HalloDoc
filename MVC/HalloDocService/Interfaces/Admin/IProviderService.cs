using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IProviderService
{
    AdminProviderViewModel GetAllProviderData(string? regionId=null,string? order=null);
    // AdminProviderViewModel GetProviderByRegion(int regionId);
    void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds);

    Physician GetSingleProviderData(int Id);

    AdminPhysicianEditViewModel GetPhyisicianData(int Id);
    void UpdateProviderPassword(int Id,string Password);
    void UpdatePersonalInformation(AdminPhysicianEditViewModel viewData);
    void UpdateGeneralInformation(AdminPhysicianEditViewModel viewData);
    void UpdateBillingInfo(AdminPhysicianEditViewModel viewData);
    void UpdateBusinessInformation(AdminPhysicianEditViewModel viewData);
    string? GetFilesPath(int Id);
    string? GetPhotoFilePath(int Id);

    void UploadDocument(int Id,string FileId, string filePath);
    string GetAgreementFile(int Id,string FileId);

    void DeleteProvider(int Id);
    AdminPhysicianCreateViewModel GetRoleAndState();

}