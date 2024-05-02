using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocService.Provider.Interfaces;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.Enums;

namespace HalloDocService.Provider.Implementation;
public class ProviderInvoicingService : IProviderInvoicingService
{
    private readonly IProviderInvoicingRepo _providerInvoicingRepo;

    public ProviderInvoicingService(IProviderInvoicingRepo providerInvoicingRepo)
    {
        _providerInvoicingRepo = providerInvoicingRepo;
    }

    public TimeSheetViewModel CheckFinalizeAndGetData(string StartDate, string EndDate)
    {
        DateTime startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
        DateTime endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
        Timesheet? TimeSheetData = _providerInvoicingRepo.CheckFinalizeAndGetData(startDate, endDate);
        if (TimeSheetData == null)
        {
            return new TimeSheetViewModel();
        }
        TimeSheetViewModel timeSheetViewModel = new()
        {
            Id = TimeSheetData.Id,
            Physicianid = TimeSheetData.Physicianid,
            Startdate = TimeSheetData.Startdate,
            Enddate = TimeSheetData.Enddate,
            Isfinalized = TimeSheetData.Isfinalized,
            TimesheetdetailsList = TimeSheetData.Timesheetdetails.Select(tsd => new TimeSheetDetailsView()
            {
                Id = tsd.Id,
                Timesheetid = tsd.Timesheetid,
                Shiftdate = tsd.Shiftdate,
                Shifthours = tsd.Shifthours,
                Housecall = tsd.Housecall,
                Phoneconsult = tsd.Phoneconsult,
                Isweekend = tsd.Isweekend ?? false
            }).ToList(),
            TimesheetreimbursementsList = TimeSheetData.Timesheetreimbursements.Select(tsr => new TimesheetreimbursementView()
            {
                Id = tsr.Id,
                Timesheetid = tsr.Timesheetid,
                Item = tsr.Item,
                Amount = tsr.Amount,
                Bill = tsr.Bill,
                Shiftdate = tsr.ShiftDate
            }).ToList()
        };
        return timeSheetViewModel;
    }
    public TimeSheetViewModel GetTimeSheetList(string StartDate, string EndDate)
    {
        DateTime startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
        DateTime endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
        Timesheet? timeSheetData = _providerInvoicingRepo.CheckFinalizeAndGetData(startDate, endDate);
        List<DateTime> datesInRange = GetDatesInRange(startDate, endDate);

        TimeSheetViewModel timeSheetViewModel = new()
        {
            TimesheetdetailsList = datesInRange.Select(date => new TimeSheetDetailsView { Shiftdate = date }).ToList(),
            TimesheetreimbursementsList = datesInRange.Select(date => new TimesheetreimbursementView { Shiftdate = date }).ToList()
        };

        if (timeSheetData != null)
        {
            timeSheetViewModel.Id = timeSheetData.Id;
            timeSheetViewModel.Physicianid = timeSheetData.Physicianid;
            timeSheetViewModel.Startdate = timeSheetData.Startdate;
            timeSheetViewModel.Enddate = timeSheetData.Enddate;
            timeSheetViewModel.Isfinalized = timeSheetData.Isfinalized;

            if (timeSheetData.Timesheetdetails.Any(tsd => tsd.Timesheetid == timeSheetData.Id))
            {
                timeSheetViewModel.TimesheetdetailsList = timeSheetData.Timesheetdetails.Select(tsd => new TimeSheetDetailsView
                {
                    Id = tsd.Id,
                    Timesheetid = tsd.Timesheetid,
                    Shiftdate = tsd.Shiftdate,
                    Shifthours = tsd.Shifthours,
                    Housecall = tsd.Housecall,
                    Phoneconsult = tsd.Phoneconsult,
                    Isweekend = tsd.Isweekend ?? false
                }).ToList();
            }

            if (timeSheetData.Timesheetreimbursements.Any(tsr => tsr.Timesheetid == timeSheetData.Id))
            {
                timeSheetViewModel.TimesheetreimbursementsList = timeSheetData.Timesheetreimbursements.Select(tsr => new TimesheetreimbursementView
                {
                    Id = tsr.Id,
                    Timesheetid = tsr.Timesheetid,
                    Shiftdate = tsr.ShiftDate,
                    Item = tsr.Item,
                    Amount = tsr.Amount,
                    Bill = tsr.Bill
                }).ToList();
            }
        }

        return timeSheetViewModel;
    }


    public static List<DateTime> GetDatesInRange(DateTime startDate, DateTime endDate)
    {
        List<DateTime> datesInRange = new();

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            datesInRange.Add(date);
        }

        return datesInRange;
    }

    public void AddUpdateTimeSheet(TimeSheetViewModel timesheetDetailsList)
    {
        if (timesheetDetailsList.Id == 0) // Add New Record if TimesheetId is 0
        {
            Timesheet newTimeSheet = new()
            {
                Physicianid = timesheetDetailsList.Physicianid,
                Startdate = timesheetDetailsList.Startdate,
                Enddate = timesheetDetailsList.Enddate,
                Isfinalized = timesheetDetailsList.Isfinalized,
                Status = "Pending"
            };
            _providerInvoicingRepo.AddTimeSheet(newTimeSheet);

            List<Timesheetdetail> newTimesheetsDetails = new();
            foreach (var item in timesheetDetailsList.TimesheetdetailsList)
            {
                newTimesheetsDetails.Add(new Timesheetdetail()
                {
                    Timesheetid = newTimeSheet.Id,
                    Shiftdate = item.Shiftdate,
                    Shifthours = item.Shifthours,
                    Housecall = item.Housecall,
                    Phoneconsult = item.Phoneconsult,
                    Isweekend = item.Isweekend
                });
            }
            _providerInvoicingRepo.AddTimeSheetDetails(newTimesheetsDetails);
        }
        else // Update If Exists
        {
            foreach (var item in timesheetDetailsList.TimesheetdetailsList)
            {
                Timesheetdetail? existingDetail = _providerInvoicingRepo.GetTimesheetDetailById(item.Id);
                if (existingDetail != null)
                {
                    existingDetail.Shiftdate = item.Shiftdate;
                    existingDetail.Shifthours = item.Shifthours;
                    existingDetail.Housecall = item.Housecall;
                    existingDetail.Phoneconsult = item.Phoneconsult;
                    existingDetail.Isweekend = item.Isweekend;

                    _providerInvoicingRepo.UpdateTimesheetDetail(existingDetail);
                }
            }
        }
    }

    public void Finalize(int Id)
    {
        _providerInvoicingRepo.Finalize(Id);
    }
}