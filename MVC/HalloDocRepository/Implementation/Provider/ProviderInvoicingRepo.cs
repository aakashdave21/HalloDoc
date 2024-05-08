using HalloDocRepository.DataModels;
using Microsoft.EntityFrameworkCore;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Enums;


namespace HalloDocRepository.Provider.Implementation;
public class ProviderInvoicingRepo : IProviderInvoicingRepo
{

    private readonly HalloDocContext _dbContext;

    public ProviderInvoicingRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Timesheet? CheckFinalizeAndGetData(DateTime StartDate, DateTime EndDate, int PhysicianId = 0)
    {
        Timesheet? isTimeSheetExits = _dbContext.Timesheets.Include(ts => ts.Timesheetdetails).Include(ts => ts.Timesheetreimbursements).FirstOrDefault(ts => ts.Startdate == StartDate && ts.Enddate == EndDate && ts.Physicianid == PhysicianId);

        return isTimeSheetExits;
    }

    public void AddTimeSheet(Timesheet newTimeSheet)
    {
        _dbContext.Timesheets.Add(newTimeSheet);
        _dbContext.SaveChanges();
    }

    public void AddTimeSheetDetails(List<Timesheetdetail> newTimesheetsDetails)
    {
        _dbContext.Timesheetdetails.AddRange(newTimesheetsDetails);
        _dbContext.SaveChanges();
    }
    public Timesheetdetail? GetTimesheetDetailById(int Id)
    {
        return _dbContext.Timesheetdetails.FirstOrDefault(ts => ts.Id == Id);
    }

    public void UpdateTimesheetDetail(Timesheetdetail updatedTimeSheet)
    {
        var existingDetail = _dbContext.Timesheetdetails.FirstOrDefault(d => d.Id == updatedTimeSheet.Id);

        if (existingDetail != null)
        {
            existingDetail.Shiftdate = updatedTimeSheet.Shiftdate;
            existingDetail.Shifthours = updatedTimeSheet.Shifthours;
            existingDetail.Housecall = updatedTimeSheet.Housecall;
            existingDetail.Phoneconsult = updatedTimeSheet.Phoneconsult;
            existingDetail.Isweekend = updatedTimeSheet.Isweekend;

            _dbContext.SaveChanges();
            return;
        }
        throw new RecordNotFoundException();
    }

    public void Finalize(int Id)
    {
        Timesheet? isTimeSheetExits = _dbContext.Timesheets.FirstOrDefault(ts => ts.Id == Id);
        if (isTimeSheetExits != null)
        {
            isTimeSheetExits.Isfinalized = true;
            _dbContext.SaveChanges();
            return;
        }
        throw new RecordNotFoundException();
    }

    public void AddTimeSheetReimbursement(Timesheetreimbursement timesheetreimbursement)
    {
        if (timesheetreimbursement.Id != 0)
        { // Perform Update
            Timesheetreimbursement? reimburshementDetails = _dbContext.Timesheetreimbursements.FirstOrDefault(t => t.Id == timesheetreimbursement.Id);
            if (reimburshementDetails != null)
            {
                reimburshementDetails.Item = timesheetreimbursement.Item;
                reimburshementDetails.Amount = timesheetreimbursement.Amount;
                _dbContext.SaveChanges();
            }
        }
        else
        { //Perform Addition
            _dbContext.Timesheetreimbursements.Add(timesheetreimbursement);
            _dbContext.SaveChanges();
        }
    }

    public void DeleteTimeReimbursement(int Id)
    {
        if (Id != 0)
        {
            Timesheetreimbursement? reimburshementDetails = _dbContext.Timesheetreimbursements.FirstOrDefault(t => t.Id == Id);
            if (reimburshementDetails != null)
            {
                _dbContext.Timesheetreimbursements.Remove(reimburshementDetails);
                _dbContext.SaveChanges();
                return;
            }
        }
        throw new RecordNotFoundException();
    }

    public Timesheet GetTimeSheetById(int Id)
    {
        Timesheet? timesheet = _dbContext.Timesheets.FirstOrDefault(t => t.Id == Id);
        if (timesheet != null) return timesheet;
        throw new RecordNotFoundException();
    }

    
}