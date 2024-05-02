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

    public Timesheet? CheckFinalizeAndGetData(DateTime StartDate, DateTime EndDate){
        Timesheet? isTimeSheetExits = _dbContext.Timesheets.Include(ts => ts.Timesheetdetails).Include(ts => ts.Timesheetreimbursements).FirstOrDefault(ts => ts.Startdate == StartDate && ts.Enddate == EndDate);
        return isTimeSheetExits;
    }

    public void AddTimeSheet(Timesheet newTimeSheet){
        _dbContext.Timesheets.Add(newTimeSheet);
        _dbContext.SaveChanges();
    }

    public void AddTimeSheetDetails(List<Timesheetdetail> newTimesheetsDetails){
        _dbContext.Timesheetdetails.AddRange(newTimesheetsDetails);
        _dbContext.SaveChanges();
    }
    public Timesheetdetail? GetTimesheetDetailById(int Id){
        return _dbContext.Timesheetdetails.FirstOrDefault(ts => ts.Id == Id);
    }

    public void UpdateTimesheetDetail(Timesheetdetail updatedTimeSheet){
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
}