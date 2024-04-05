using HalloDocRepository.Admin.Implementation;
using HalloDocRepository.DataModels;

namespace HalloDocRepository.Admin.Interfaces;
public interface IScheduleRepo
{ 
    IEnumerable<Shiftdetail> ShiftsLists(string startDate,string endDate,string? status = null);
    void CreateShift(Shift shiftData);
    void CreateShiftDetails(Shiftdetail shiftData);
    void CreateShiftDetailsList(List<Shiftdetail> shiftDetailsList);
    IEnumerable<Shiftdetail> GetShiftByProviderId(int phyId);
    void UpdateShift(int shiftId,DateTime shiftDate,TimeOnly startTime,TimeOnly endTime,int aspUserId);
    void ChangeStatus(int shiftId,int AspUserId);
    void Delete(int shiftId,int AspUserId);

    IEnumerable<Physician> GetCallPhysician(int regionId = 0);
    ReviewShiftPage ReviewShift(int RegionId=0,int PageSize=5,int PageNum=1);
    
    void UpdateShift(List<int> shiftDetailIds,int AspUserId,string IsDelete);
}