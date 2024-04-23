using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace HalloDocRepository.Admin.Implementation;
public class ScheduleRepo : IScheduleRepo
{
    private readonly HalloDocContext _dbContext;
    public ScheduleRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Shiftdetail> ShiftsLists(string startDate, string endDate, string? status = null, int? PhyId = null)
    {
        if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
       !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
        {
            throw new ArgumentException("Invalid date format", nameof(startDate));
        }

        IQueryable<Shiftdetail> shiftdetailsInfo = _dbContext.Shiftdetails
                .Include(sd => sd.Shift).ThenInclude(s => s.Physician)
                .Include(sd => sd.Region)
                .Where(sf => (PhyId==null || sf.Shift.Physicianid == PhyId) && sf.Shiftdate.Date >= parsedStartDate.ToDateTime(new TimeOnly()).Date &&
                                sf.Shiftdate.Date <= parsedEndDate.ToDateTime(new TimeOnly()).Date && sf.Isdeleted == false);

        if (!string.IsNullOrEmpty(status))
        {
            shiftdetailsInfo = shiftdetailsInfo.Where(sf => sf.Status == short.Parse(status));
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

    public IEnumerable<Physician> GetCallPhysician(int regionId = 0){

        IQueryable<Physician> query = _dbContext.Physicians.Where(phy => phy.OnCallStatus == 1 || phy.OnCallStatus == 2);
        if(regionId!=0){
            List<int?> PhyId = _dbContext.Physicianregions.Where(phyReg => phyReg.Regionid == regionId).Select(phy => phy.Physicianid).ToList();
            query = query.Where(phy => PhyId.Contains(phy.Id));
        }
        return query.ToList();
    }
    public ReviewShiftPage ReviewShift(int RegionId=0,int PageSize=5,int PageNum=1){
        IQueryable<Shiftdetail> query = _dbContext.Shiftdetails.Include(sd=>sd.Region).Include(sd=>sd.Shift).ThenInclude(s=>s.Physician).Where(sd => sd.Isdeleted == false && sd.Status == 1);
        if(RegionId!=0){
            query = query.Where(sd => sd.Regionid == RegionId);
        }
        query = query.OrderBy(sd => sd.Shift.Physician.Firstname);
        
        ReviewShiftPage reviewPage = new(){
            TotalPage = query.Count(),
            ShiftListsInfo = query.Skip((PageNum-1)*PageSize).Take(PageSize).ToList()
        };
        return reviewPage;
    }

    public void UpdateShift(List<int> shiftDetailIds,int AspUserId,string IsDelete){
        foreach(int shiftDetailId in shiftDetailIds){
            Shiftdetail? shiftDetail = _dbContext.Shiftdetails.FirstOrDefault(sd => sd.Id == shiftDetailId && sd.Isdeleted == false);
            if (shiftDetail!= null)
            {
                if(!string.IsNullOrEmpty(IsDelete) && IsDelete == "true"){
                    shiftDetail.Isdeleted = true;
                }
                if(!string.IsNullOrEmpty(IsDelete) && IsDelete == "false"){
                    shiftDetail.Status = 2;
                }
                shiftDetail.Modifiedby = AspUserId;
                shiftDetail.Updatedat = DateTime.Now;

                _dbContext.SaveChanges();
            }else{
                throw new Exception($"The shift detail with id: {shiftDetailId} does not exist.");
            }
        }
    }

    public IEnumerable<Region?>? GetRegionByPhysician(int? PhyId){
        return _dbContext?.Physicianregions?.Where(phyReg => phyReg.Physicianid == PhyId)?.Select(phyReg => phyReg.Region)?.ToList();
    }
}

public class ReviewShiftPage{
    public int TotalPage {get; set;}
    public IEnumerable<Shiftdetail> ShiftListsInfo {get; set;} = new List<Shiftdetail>();
}