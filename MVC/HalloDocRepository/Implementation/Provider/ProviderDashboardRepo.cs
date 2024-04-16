using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using HalloDocRepository.Provider.Interfaces;


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
            "new" => _dbContext.Requests.Include(req => req.Requestclients).Where(req => req.Physicianid == PhysicianId && req.Status == 1 && req.Isdeleted != true && req.IsBlocked != true),
            "pending" => _dbContext.Requests.Include(req => req.Requestclients).Where(req => req.Physicianid == PhysicianId && req.Status == 2 && req.Isdeleted != true && req.IsBlocked != true),
            "active" => _dbContext.Requests.Include(req => req.Requestclients).Where(req => req.Physicianid == PhysicianId && (req.Status == 4 || req.Status == 5) && req.Isdeleted != true && req.IsBlocked != true),
            "conclude" => _dbContext.Requests.Include(req => req.Requestclients).Where(req => req.Physicianid == PhysicianId && req.Status == 6 && req.Isdeleted != true && req.IsBlocked != true),
            _ => _dbContext.Requests.Include(req => req.Requestclients).Where(req => req.Physicianid == PhysicianId && req.Status == 1 && req.Isdeleted != true && req.IsBlocked != true),
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
            NewCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == 1 && req.Isdeleted != true && req.IsBlocked != true),
            PendingCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == 2 && req.Isdeleted != true && req.IsBlocked != true),
            ActiveCount = group.Count(req => req.Physicianid == PhysicianId && (req.Status == 4 || req.Status == 5) && req.Isdeleted != true && req.IsBlocked != true),
            ConcludeCount = group.Count(req => req.Physicianid == PhysicianId && req.Status == 6 && req.Isdeleted != true && req.IsBlocked != true),
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
            query.Status = 2;
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception("Request Not Found!");

    }
}