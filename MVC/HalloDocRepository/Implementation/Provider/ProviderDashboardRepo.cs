using HalloDocRepository.DataModels;
using Microsoft.EntityFrameworkCore;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Enums;


namespace HalloDocRepository.Provider.Implementation;
public class ProviderDashboardRepo : IProviderDashboardRepo
{

    private readonly HalloDocContext _dbContext;

    public ProviderDashboardRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (List<Request> req, int totalCount) GetDashboardRequests(string status, string searchBy = "", int reqTypeId = 0, int pageNumber = 1, int pageSize = 2, int AspId = 0)
    {
        int? PhysicianId = _dbContext?.Physicians?.FirstOrDefault(req => req.Aspnetuserid == AspId)?.Id;
        IQueryable<Request> query = status switch
        {
            "new" => _dbContext.Requests.Include(req => req.Requestclients).Include(req=>req.Encounterform).Where(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Unassigned && req.Isdeleted != true && req.IsBlocked != true),
            "pending" => _dbContext.Requests.Include(req => req.Requestclients).Include(req=>req.Encounterform).Where(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Accepted && req.Isdeleted != true && req.IsBlocked != true),
            "active" => _dbContext.Requests.Include(req => req.Requestclients).Include(req=>req.Encounterform).Where(req => req.Physicianid == PhysicianId && (req.Status == (short)RequestStatusEnum.MdRequest || req.Status == (short)RequestStatusEnum.MDONSite) && req.Isdeleted != true && req.IsBlocked != true),
            "conclude" => _dbContext.Requests.Include(req => req.Requestclients).Include(req=>req.Encounterform).Where(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Conclude && req.Isdeleted != true && req.IsBlocked != true),
            _ => _dbContext.Requests.Include(req => req.Requestclients).Include(req=>req.Encounterform).Where(req => req.Physicianid == PhysicianId && req.Status == 1 && req.Isdeleted != true && req.IsBlocked != true),
        };
        if (reqTypeId > 0)
        {
            query = query.Where(req => req.Requesttypeid == reqTypeId);
        }
        if (!string.IsNullOrWhiteSpace(searchBy))
        {
            query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
        }
        query = query.OrderByDescending(req => req.Createdat);

        int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(), totalCount);
    }

    public Dictionary<string, int> CountRequestByType(int AspId){
        Dictionary<string,int>? CountRequestRow = new();
        int? PhysicianId = _dbContext?.Physicians?.FirstOrDefault(req => req.Aspnetuserid == AspId)?.Id;
        CountRequestRow = _dbContext.Requests
        .GroupBy(req => true) // Group all records into one group
        .Select(group => new
        {
            NewCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Unassigned && req.Isdeleted != true && req.IsBlocked != true),
            PendingCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Accepted && req.Isdeleted != true && req.IsBlocked != true),
            ActiveCount = group.Count(req => req.Physicianid == PhysicianId && (req.Status == (short)RequestStatusEnum.MdRequest || req.Status == (short)RequestStatusEnum.MDONSite) && req.Isdeleted != true && req.IsBlocked != true),
            ConcludeCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == (short)RequestStatusEnum.Conclude && req.Isdeleted != true && req.IsBlocked != true),
        })
        .Select(result => new Dictionary<string, int>
        {
            { "new", result.NewCount },
            { "pending", result.PendingCount },
            { "active", result.ActiveCount },
            { "conclude", result.ConcludeCount },
        })
        .SingleOrDefault();

        return CountRequestRow;
    }

    public void AcceptRequest(int ReqId){
        Request query = _dbContext.Requests.FirstOrDefault(req => req.Id == ReqId);
        if(query!=null){
            query.Status = (short)RequestStatusEnum.Accepted;
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Request Not Found!");

    }

    public bool CheckEncounterFinalized(int ReqId){
        Encounterform? encouterFormData = _dbContext.Encounterforms.FirstOrDefault(enc => enc.RequestId == ReqId);
        if(encouterFormData!=null){
            return encouterFormData.Isfinalized == true; 
        }   
        return false;
    }

    public void FinalizeForm(int EncId, int ReqId){
        Encounterform? encForm = _dbContext.Encounterforms.FirstOrDefault(enc => enc.Id == EncId && enc.RequestId == ReqId);
        if(encForm!=null){
            encForm.Isfinalized = true;
            encForm.Finalizeddate = DateTime.Now;
            encForm.Updatedat = DateTime.Now;

            _dbContext.SaveChanges();
            return;
        }
        throw new RecordNotFoundException(); 
    }

    public Aspnetuser? SendProfileRequest(int? PhyId){
        Physician? physician = _dbContext.Physicians.FirstOrDefault(phy => phy.Id == PhyId);
        if(physician!=null && physician?.Createdby!=null){
            return _dbContext.Aspnetusers.Include(user=>user.AdminAspnetusers).FirstOrDefault(user => user.Id == physician.Createdby);
        }
        return null;
    }

}