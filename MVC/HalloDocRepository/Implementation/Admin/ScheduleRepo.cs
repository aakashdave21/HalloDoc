using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing;


namespace HalloDocRepository.Admin.Implementation;
public class ScheduleRepo : IScheduleRepo
{
    private readonly HalloDocContext _dbContext;
    public ScheduleRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Shiftdetail> ShiftsLists(string startDate, string endDate, string? status = null)
    {
        if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
       !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
        {
            throw new ArgumentException("Invalid date format", nameof(startDate));
        }

        IEnumerable<Shiftdetail> shiftdetailsInfo = _dbContext.Shiftdetails
            .Include(sd => sd.Shift).ThenInclude(s => s.Physician)
            .Include(sd => sd.Region)
            .Where(sf => sf.Shiftdate.Date >= parsedStartDate.ToDateTime(new TimeOnly()).Date &&
                            sf.Shiftdate.Date <= parsedEndDate.ToDateTime(new TimeOnly()).Date && sf.Isdeleted == false)
            .ToList();

        if (!string.IsNullOrEmpty(status))
        {
            shiftdetailsInfo = shiftdetailsInfo.Where(sf => sf?.Status == short.Parse(status));
        }
        return shiftdetailsInfo;
    }

    public void CreateShift(Shift shiftData)
    {
        _dbContext.Shifts.Add(shiftData);
        _dbContext.SaveChanges();
    }
    public void CreateShiftDetails(Shiftdetail shiftData)
    {
        _dbContext.Shiftdetails.Add(shiftData);
        _dbContext.SaveChanges();
    }
    public void CreateShiftDetailsList(List<Shiftdetail> shiftDetailsList)
    {
        _dbContext.Shiftdetails.AddRange(shiftDetailsList);
        _dbContext.SaveChanges();
    }
    public IEnumerable<Shiftdetail> GetShiftByProviderId(int phyId)
    {
        List<int> listOfShifts = _dbContext.Shifts.Where(shift => shift.Physicianid == phyId).Select(shift => shift.Id).ToList();
        return _dbContext.Shiftdetails.Where(sd => listOfShifts.Contains(sd.Shiftid)).ToList();
    }
    public void UpdateShift(int shiftId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, int aspUserId)
    {
        Shiftdetail? shiftInfo = _dbContext.Shiftdetails.FirstOrDefault(sd => sd.Id == shiftId && sd.Isdeleted == false);
        if (shiftInfo != null)
        {
            shiftInfo.Shiftdate = shiftDate;
            shiftInfo.Starttime = startTime;
            shiftInfo.Endtime = endTime;
            shiftInfo.Modifiedby = aspUserId;
            shiftInfo.Updatedat = DateTime.Now;

            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Invalid shift");
    }

    public void ChangeStatus(int shiftId,int AspUserId){
        Shiftdetail? shiftInfo = _dbContext.Shiftdetails.FirstOrDefault(sd => sd.Id == shiftId && sd.Isdeleted == false);
        if (shiftInfo != null)
        {
            shiftInfo.Status = (short)(shiftInfo.Status == 1 ? 2 : 1);
            shiftInfo.Modifiedby = AspUserId;
            shiftInfo.Updatedat = DateTime.Now;

            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Invalid shift");   
    }
    public void Delete(int shiftId,int AspUserId){
        Shiftdetail? shiftInfo = _dbContext.Shiftdetails.FirstOrDefault(sd => sd.Id == shiftId && sd.Isdeleted == false);
        if (shiftInfo != null)
        {
            shiftInfo.Isdeleted = true;
            shiftInfo.Modifiedby = AspUserId;
            shiftInfo.Updatedat = DateTime.Now;

            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Invalid shift");   
    }
}