using HalloDocRepository.DataModels;

namespace HalloDocRepository.Provider.Interfaces;
public interface IProviderInvoicingRepo
{
    Timesheet? CheckFinalizeAndGetData(DateTime StartDate, DateTime EndDate);
    void AddTimeSheet(Timesheet newTimeSheet);
    void AddTimeSheetDetails(List<Timesheetdetail> newTimesheetsDetails);
    Timesheetdetail? GetTimesheetDetailById(int Id);
    void UpdateTimesheetDetail(Timesheetdetail updatedTimeSheet);
}