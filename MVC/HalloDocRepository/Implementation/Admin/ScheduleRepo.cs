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

    public IEnumerable<Shiftdetail> ShiftsLists(string startDate,string endDate,string? status = null)
    {
         if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) || 
        !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
        {
            throw new ArgumentException("Invalid date format", nameof(startDate));
        }
        
        IEnumerable<Shiftdetail> shiftdetailsInfo = _dbContext.Shiftdetails
    .Include(sd => sd.Shift)
    .Where(sf => sf.Shiftdate.Date >= parsedStartDate.ToDateTime(new TimeOnly()).Date &&
                     sf.Shiftdate.Date <= parsedEndDate.ToDateTime(new TimeOnly()).Date)
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
}