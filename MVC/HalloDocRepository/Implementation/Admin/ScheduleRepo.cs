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

    public IEnumerable<Shift> ShiftsLists(string startDate,string? status = null){
        if (!DateOnly.TryParse(startDate, out DateOnly parsedDate))
        {
            throw new ArgumentException("Invalid date format", nameof(startDate));
        }
        Console.WriteLine(status+"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        IEnumerable<Shift> shiftDetails = _dbContext.Shifts.Include(sd => sd.Shiftdetail).Where(sf => sf.Startdate == parsedDate).ToList();
        if(!string.IsNullOrEmpty(status)){
            shiftDetails = shiftDetails.Where(sf => sf?.Shiftdetail?.Status == short.Parse(status));
        }
        return shiftDetails;
    }
}