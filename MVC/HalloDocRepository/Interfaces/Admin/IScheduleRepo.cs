using HalloDocRepository.DataModels;

namespace HalloDocRepository.Admin.Interfaces;
public interface IScheduleRepo
{ 
    IEnumerable<Shiftdetail> ShiftsLists(string startDate,string endDate,string? status = null);
    void CreateShift(Shift shiftData);
    void CreateShiftDetails(Shiftdetail shiftData);
    void CreateShiftDetailsList(List<Shiftdetail> shiftDetailsList);
}