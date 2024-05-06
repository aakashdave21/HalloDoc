using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HalloDocService.Provider.Interfaces;
public interface IProviderInvoicingService
{
    TimeSheetViewModel CheckFinalizeAndGetData(string StartDate, string EndDate);
    TimeSheetViewModel GetTimeSheetList(string StartDate, string EndDate);
    void AddUpdateTimeSheet(TimeSheetViewModel timesheetDetailsList);
    void Finalize(int Id);
    void AddUpdateTimeReimbursement(TimeSheetViewModel timesheetDetail,TimesheetreimbursementView timesheetreimbursement);

    void DeleteTimeReimbursement(int Id);
}