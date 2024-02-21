
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Admin.Implementation;
public class AdminDashboardRepo : IAdminDashboardRepo
{

    private readonly HalloDocContext? _dbContext;

    public AdminDashboardRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Request> GetNewRequest()
    {
        return _dbContext.Requests
                     .Include(req => req.Requestclients) // Include related Requestclients
                     .Where(req => req.Status == 1)
                     .ToList();
    }
    public IEnumerable<Request> GetPendingStatusRequest()
    {
        return _dbContext.Requests
                     .Include(req => req.Requestclients) // Include related Requestclients
                     .Include(req => req.Physician)
                     .Where(req => req.Status == 2 && req.Accepteddate == null) // <---HERE -> ADD ALSO PHYSICAIN IS NOT NULL
                     .ToList();
    }
    public IEnumerable<Request> GetActiveStatusRequest()
    {
        return _dbContext.Requests
                     .Include(req => req.Requestclients) // Include related Requestclients
                     .Include(req => req.Physician)
                     .Where(req => req.Status == 2 && req.Accepteddate != null) // <---HERE -> ADD ALSO PHYSICAIN IS NOT NULL
                     .ToList();
    }
    public IEnumerable<Request> GetConcludeStatusRequest()
    {
        return _dbContext.Requests
                     .Include(req => req.Requestclients) // Include related Requestclients
                     .Include(req => req.Physician)
                     .Where(req => req.Completedbyphysician == true && req.Status != 1) // <---HERE -> ADD ALSO PHYSICAIN IS NOT NULL
                     .ToList();
    }
    public IEnumerable<Request> GetCloseStatusRequest()
    {
        return _dbContext.Requests
                         .Include(req => req.Requestclients) // Include related Requestclients
                             .ThenInclude(rc => rc.Region) // Include related Region for each Requestclient
                         .Include(req => req.Physician)
                         .Where(req => req.Status == 8)
                         .ToList();
    }
    public IEnumerable<Request> GetUnpaidStatusRequest()
    {
        return _dbContext.Requests
                     .Include(req => req.Requestclients) // Include related Requestclients
                     .Include(req => req.Physician)
                     .Where(req => req.Status == 4 || req.Status==11 || req.Status==16) // <---Reserving | Consult | Unpaid
                     .ToList();
    }
    public Dictionary<string,int> CountRequestByType()
    {
        Dictionary<string,int> CountRequestRow = new Dictionary<string, int>();
        CountRequestRow = _dbContext.Requests
        .GroupBy(req => true) // Group all records into one group
        .Select(group => new
        {
            NewCount = group.Count(req => req.Status == 1),
            PendingCount = group.Count(req => req.Status == 2 && req.Accepteddate == null),
            ActiveCount = group.Count(req => req.Status == 2 && req.Accepteddate != null),
            ConcludeCount = group.Count(req => req.Completedbyphysician == true && req.Status != 1),
            CloseCount = group.Count(req => req.Status == 8),
            UnpaidCount = group.Count(req => req.Status == 4 || req.Status == 11 || req.Status == 16)
        })
        .Select(result => new Dictionary<string, int>
        {
            { "new", result.NewCount },
            { "pending", result.PendingCount },
            { "active", result.ActiveCount },
            { "conclude", result.ConcludeCount },
            { "close", result.CloseCount },
            { "unpaid", result.UnpaidCount }
        })
        .SingleOrDefault();

        return CountRequestRow;
    }

}
