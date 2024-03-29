using HalloDocRepository.DataModels;

namespace HalloDocRepository.Admin.Interfaces;
public interface IScheduleRepo
{ 
    IEnumerable<Shift> ShiftsLists(string startDate,string? status = null);
}