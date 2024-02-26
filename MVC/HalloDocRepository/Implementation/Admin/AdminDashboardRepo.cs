
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

    public IEnumerable<Request> GetNewRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Where(req => req.Status == 1);

    if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
    

    return query.ToList();
}

public IEnumerable<Request> GetPendingStatusRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Include(req => req.Physician)
        .Where(req => req.Status == 2 && req.Accepteddate == null);

    if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
    

    return query.ToList();
}

public IEnumerable<Request> GetActiveStatusRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Include(req => req.Physician)
        .Where(req => (req.Status == 4 || req.Status == 5) && req.Accepteddate != null);

    if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
    

    return query.ToList();
}

public IEnumerable<Request> GetConcludeStatusRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Include(req => req.Physician)
        .Where(req => req.Status == 6);

    if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
    

    return query.ToList();
}

public IEnumerable<Request> GetCloseStatusRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .ThenInclude(rc => rc.Region)
        .Include(req => req.Physician)
        .Where(req => (req.Status == 8 || req.Status == 7 || req.Status == 3));

     if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
   

    return query.ToList();
}

public IEnumerable<Request> GetUnpaidStatusRequest(string searchBy = "",int reqTypeId=0)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Include(req => req.Physician)
        .Where(req => req.Status == 9);

    if(reqTypeId>0){
        query = query.Where(req => req.Requesttypeid == reqTypeId);
        Console.WriteLine(query);
    }
    if (!string.IsNullOrWhiteSpace(searchBy))
    {
        query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchBy)));
    }
    

    return query.ToList();
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
            ActiveCount = group.Count(req => (req.Status == 4 || req.Status == 5) && req.Accepteddate != null),
            ConcludeCount = group.Count(req => req.Status == 6),
            CloseCount = group.Count(req =>  req.Status == 8 || req.Status == 7 || req.Status == 3),
            UnpaidCount = group.Count(req => req.Status == 9)
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

    public Request GetViewCaseDetails(int id) {
        return _dbContext.Requests.Include(item => item.Requestclients)
                                        .ThenInclude(req => req.Region)
                                    .Include(req=>req.Requesttype)
                                    .FirstOrDefault(req => req.Id == id);
    }

    public Requestnote GetViewNotesDetails(int reqId){
        return _dbContext.Requestnotes.FirstOrDefault(req => req.Requestid == reqId);
    }
    public Requestclient GetPatientNoteDetails(int reqId){
        return _dbContext.Requestclients.FirstOrDefault(req=> req.Requestid == reqId);
    }
    // public Dictionary<string,string> GetAllCancelNotes(int reqId){
    //     var cancelNotes = _dbContext.Requeststatuslogs
    //     .Where(log => log.Requestid == reqId && (log.Status == 7 || log.Status == 3))
    //     .GroupBy(
    //         log => 1, // Group by a constant value to ensure a single group
    //         (key, logs) => new
    //         {
    //             PatientCancel = logs.FirstOrDefault(log => log.Status == 7)?.Notes,
    //             AdminCancel = logs.FirstOrDefault(log => log.Status == 3 && log.Adminid != null && log.Physicianid == null)?.Notes,
    //             PhysicianCancel = logs.FirstOrDefault(log => log.Status == 3 && log.Adminid == null && log.Physicianid != null)?.Notes
    //         })
    //     .Select(result => new Dictionary<string, string>
    //     {
    //         { "PatientCancel", result.PatientCancel },
    //         { "AdminCancel", result.AdminCancel },
    //         { "PhysicianCancel", result.PhysicianCancel }
    //     })
    //     .FirstOrDefault();

    //     return cancelNotes ?? new Dictionary<string, string>();

    // }
    public void SaveAdditionalNotes(string AdditionalNote,int noteId){
        var notesData = _dbContext.Requestnotes.FirstOrDefault(req=>req.Id == noteId);
        if(notesData!=null){
            notesData.Adminnotes = AdditionalNote;
            _dbContext.SaveChanges();
        }else{
            throw new Exception();
        }
    }

}
