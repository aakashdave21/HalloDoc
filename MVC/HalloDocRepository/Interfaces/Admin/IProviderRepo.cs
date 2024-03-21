using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;
using AdminTable = HalloDocRepository.DataModels.Admin;

namespace HalloDocRepository.Admin.Interfaces;
public interface IProviderRepo
{ 
    IEnumerable<Physician> GetAllPhysician(bool order=true,string? regionId=null);
    void UpdateNotification(List<string> stopNotificationIds,List<string> startNotificationIds);
}