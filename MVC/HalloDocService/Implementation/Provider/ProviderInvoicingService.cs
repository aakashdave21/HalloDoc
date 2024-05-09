using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocService.Provider.Interfaces;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Admin.Interfaces;

namespace HalloDocService.Provider.Implementation;
public class ProviderInvoicingService : IProviderInvoicingService
{
    private readonly IProviderInvoicingRepo _providerInvoicingRepo;
    private readonly IProviderRepo _providerRepo;

    public ProviderInvoicingService(IProviderInvoicingRepo providerInvoicingRepo,IProviderRepo providerRepo)
    {
        _providerInvoicingRepo = providerInvoicingRepo;
        _providerRepo = providerRepo;
    }

    public TimeSheetViewModel CheckFinalizeAndGetData(string StartDate, string EndDate, int PhysicianId = 0)
    {
        DateTime startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);
        DateTime endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);
        Timesheet? TimeSheetData = _providerInvoicingRepo.CheckFinalizeAndGetData(startDate, endDate, PhysicianId);
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
            Status = TimeSheetData.Status,
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
    public TimeSheetViewModel GetTimeSheetList(string? StartDate, string? EndDate,int physicianid = 0)
    {
        DateTime startDate = DateTime.ParseExact(StartDate ?? DateTime.MinValue.ToString(), "dd/MM/yyyy", null);
        DateTime endDate = DateTime.ParseExact(EndDate ?? DateTime.MinValue.ToString(), "dd/MM/yyyy", null);
        Timesheet? timeSheetData = _providerInvoicingRepo.CheckFinalizeAndGetData(startDate, endDate, physicianid);
        List<DateTime> datesInRange = GetDatesInRange(startDate, endDate);

        TimeSheetViewModel timeSheetViewModel = new()
        {
            TimesheetdetailsList = datesInRange.Select(date => new TimeSheetDetailsView { Shiftdate = date }).ToList(),
            TimesheetreimbursementsList = datesInRange.Select(date => new TimesheetreimbursementView { Shiftdate = date }).ToList()
        };

        Payrate? payrateData = _providerRepo.GetPayrateDetails(physicianid);
        if(payrateData != null){
            timeSheetViewModel.PayrateDetails.Id = payrateData.Id;
            timeSheetViewModel.PayrateDetails.Nightshiftweekend = payrateData.Nightshiftweekend;
            timeSheetViewModel.PayrateDetails.Shift = payrateData.Shift;
            timeSheetViewModel.PayrateDetails.Housecallnightweekend = payrateData.Housecallnightweekend;
            timeSheetViewModel.PayrateDetails.Housecall = payrateData.Housecall;
            timeSheetViewModel.PayrateDetails.Phoneconsult = payrateData.Phoneconsult;
            timeSheetViewModel.PayrateDetails.Phoneconsultnightweekend = payrateData.Phoneconsultnightweekend;
            timeSheetViewModel.PayrateDetails.Batchtesting = payrateData.Batchtesting;
            timeSheetViewModel.PayrateDetails.Physicianid = payrateData.Physicianid;
        }

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
                }).OrderBy(s=>s.Shiftdate).ToList();
            }

            if (timeSheetData.Timesheetreimbursements.Any(tsr => tsr.Timesheetid == timeSheetData.Id))
            {

                var mergedReimbursements = datesInRange.Select(date =>
                {
                    var reimbursement = timeSheetData.Timesheetreimbursements.FirstOrDefault(tsr => tsr.ShiftDate!=null && tsr.ShiftDate.Value.Date == date.Date);
                    if (reimbursement != null)
                    {
                        return new TimesheetreimbursementView
                        {
                            Id = reimbursement.Id,
                            Timesheetid = reimbursement.Timesheetid,
                            Shiftdate = reimbursement.ShiftDate,
                            Item = reimbursement.Item,
                            Amount = reimbursement.Amount,
                            Bill = reimbursement.Bill
                        };
                    }
                    else
                    {
                        return new TimesheetreimbursementView { Shiftdate = date };
                    }
                }).ToList();
                timeSheetViewModel.TimesheetreimbursementsList = mergedReimbursements;
            }
        }
        timeSheetViewModel.TimesheetdetailsList = timeSheetViewModel.TimesheetdetailsList.OrderBy(ts => ts.Shiftdate).ToList();
        timeSheetViewModel.TimesheetreimbursementsList = timeSheetViewModel.TimesheetreimbursementsList.OrderBy(ts => ts.Shiftdate).ToList();

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
        List<Timesheetdetail> newTimesheetsDetails = new();
        if (timesheetDetailsList.Id == 0) // Add New Record if TimesheetId is 0
        {
            Timesheet newTimeSheet = new()
            {
                Physicianid = timesheetDetailsList.Physicianid,
                Startdate = timesheetDetailsList.Startdate,
                Enddate = timesheetDetailsList.Enddate,
                Isfinalized = timesheetDetailsList.Isfinalized,
                Status = "pending"
            };
            _providerInvoicingRepo.AddTimeSheet(newTimeSheet);

            foreach (var item in timesheetDetailsList.TimesheetdetailsList)
            {
                newTimesheetsDetails.Add(new Timesheetdetail()
                {
                    Timesheetid = newTimeSheet.Id,
                    Shiftdate = item.Shiftdate,
                    Shifthours = item.Shifthours ?? 0,
                    Housecall = item.Housecall ?? 0,
                    Phoneconsult = item.Phoneconsult ?? 0,
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
                    existingDetail.Shifthours = item.Shifthours ?? 0;
                    existingDetail.Housecall = item.Housecall ?? 0;
                    existingDetail.Phoneconsult = item.Phoneconsult ?? 0;
                    existingDetail.Isweekend = item.Isweekend;
                    _providerInvoicingRepo.UpdateTimesheetDetail(existingDetail);
                }else{
                    newTimesheetsDetails.Add(new Timesheetdetail()
                    {
                        Timesheetid = timesheetDetailsList.Id,
                        Shiftdate = item.Shiftdate,
                        Shifthours = item.Shifthours ?? 0,
                        Housecall = item.Housecall ?? 0,
                        Phoneconsult = item.Phoneconsult ?? 0,
                        Isweekend = item.Isweekend
                    });
                }
            }
            if (newTimesheetsDetails.Any())
            {
                _providerInvoicingRepo.AddTimeSheetDetails(newTimesheetsDetails);
            }
        }
    }

    public void Finalize(int Id)
    {
        _providerInvoicingRepo.Finalize(Id);
    }

    public void AddUpdateTimeReimbursement(TimeSheetViewModel timesheetDetail, TimesheetreimbursementView timesheetreimbursement)
    {
        if (timesheetDetail.Id == 0) // Add New Record if TimesheetId is 0
        {
            Timesheet newTimeSheet = new()
            {
                Physicianid = timesheetDetail.Physicianid,
                Startdate = timesheetDetail.Startdate,
                Enddate = timesheetDetail.Enddate,
                Isfinalized = timesheetDetail.Isfinalized,
                Status = "pending"
            };
            _providerInvoicingRepo.AddTimeSheet(newTimeSheet);

            Timesheetreimbursement newTimeSheetDetails = new()
            {
                Timesheetid = newTimeSheet.Id,
                ShiftDate = timesheetreimbursement.Shiftdate,
                Item = timesheetreimbursement?.Item ?? "",
                Amount = timesheetreimbursement != null ? timesheetreimbursement.Amount : 0,
                Bill = timesheetreimbursement?.Bill ?? ""
            };
            _providerInvoicingRepo.AddTimeSheetReimbursement(newTimeSheetDetails);
        }
        else
        {
                Timesheetreimbursement updatedTimeSheetDetails = new()
                {
                    Id = timesheetreimbursement.Id,
                    Timesheetid = timesheetDetail.Id,
                    ShiftDate = timesheetreimbursement.Shiftdate,
                    Item = timesheetreimbursement?.Item ?? "",
                    Amount = timesheetreimbursement != null ? timesheetreimbursement.Amount : 0,
                    Bill = timesheetreimbursement?.Bill ?? ""
                };
                _providerInvoicingRepo.AddTimeSheetReimbursement(updatedTimeSheetDetails);
            }
    }

    public void DeleteTimeReimbursement(int Id){
        _providerInvoicingRepo.DeleteTimeReimbursement(Id);
    }

    public TimeSheetViewModel GetTimeSheetById(int Id){
        Timesheet TimeSheetDetails = _providerInvoicingRepo.GetTimeSheetById(Id);
        string? StartDate = TimeSheetDetails.Startdate?.ToString("dd/MM/yyyy");
        string? EndDate = TimeSheetDetails.Enddate?.ToString("dd/MM/yyyy");
        return GetTimeSheetList(StartDate, EndDate, TimeSheetDetails.Physicianid);
    }

    public void ApproveTimeSheet(int Id, int Bonus, string Description){
        Timesheet timesheetUpdate = new(){
            Id = Id,
            Bonus = Bonus,
            Description = Description,
            Status = "approved"
        };
        _providerInvoicingRepo.ApproveTimesheet(timesheetUpdate);
    }
}