using HalloDocRepository.DataModels;

namespace HalloDocRepository.Provider.Interfaces;
public interface IProviderInvoicingRepo
{
    Timesheet? CheckFinalizeAndGetData(DateTime StartDate, DateTime EndDate, int PhysicianId = 0);
    void AddTimeSheet(Timesheet newTimeSheet);
    void AddTimeSheetDetails(List<Timesheetdetail> newTimesheetsDetails);
    Timesheetdetail? GetTimesheetDetailById(int Id);
    void UpdateTimesheetDetail(Timesheetdetail updatedTimeSheet);
    void Finalize(int Id);
    void AddTimeSheetReimbursement(Timesheetreimbursement timesheetreimbursement);
    void DeleteTimeReimbursement(int Id);
    Timesheet GetTimeSheetById(int Id);
    void ApproveTimesheet(Timesheet timesheet);
}