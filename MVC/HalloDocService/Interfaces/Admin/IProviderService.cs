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
}