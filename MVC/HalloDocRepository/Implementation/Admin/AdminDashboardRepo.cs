
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Admin.Implementation;
public class AdminDashboardRepo : IAdminDashboardRepo
{

    private readonly HalloDocContext _dbContext;

    public AdminDashboardRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (IEnumerable<Request> requests, int totalCount) GetNewRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
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
        query = query.OrderByDescending(req => req.Createdat);
        
        int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(),totalCount);
    }

public (IEnumerable<Request> requests, int totalCount) GetPendingStatusRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
{
    IQueryable<Request> query = _dbContext.Requests
        .Include(req => req.Requestclients)
        .Include(req => req.Physician)
        .Where(req => req.Status == 2);

    if(reqTypeId>0){
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

        return (query.ToList(),totalCount);
}

public (IEnumerable<Request> requests, int totalCount) GetActiveStatusRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
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
            query = query.OrderByDescending(req => req.Createdat);


    int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(),totalCount);
}

public (IEnumerable<Request> requests, int totalCount) GetConcludeStatusRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
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
            query = query.OrderByDescending(req => req.Createdat);


   int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(),totalCount);
}

public (IEnumerable<Request> requests, int totalCount) GetCloseStatusRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
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
           query = query.OrderByDescending(req => req.Createdat);


   int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(),totalCount);
}

public (IEnumerable<Request> requests, int totalCount) GetUnpaidStatusRequest(string searchBy = "",int reqTypeId=0,int pageNumber=1,int pageSize=2)
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
            query = query.OrderByDescending(req => req.Createdat);


    int totalCount = query.Count();
        query = query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

        return (query.ToList(),totalCount);
}
    public Dictionary<string,int> CountRequestByType()
    {
        Dictionary<string,int> CountRequestRow = new Dictionary<string, int>();
        
        CountRequestRow = _dbContext.Requests
        .GroupBy(req => true) // Group all records into one group
        .Select(group => new
        {
            NewCount = group.Count(req => req.Status == 1),
            PendingCount = group.Count(req => req.Status == 2),
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
    public IQueryable<Requeststatuslog> GetAllCancelNotes(int reqId){
        var cancelNotes = _dbContext.Requeststatuslogs
        .Where(log => log.Requestid == reqId && (log.Status == 7 || log.Status == 3 || log.Status==2));

        return cancelNotes;

    }
    public void SaveAdditionalNotes(string AdditionalNote,int noteId,int reqId){
        if(noteId==0){
            // We have to add new Records for that
            Requestnote reqNote = new()
            {
              Requestid = reqId,
              Adminnotes = AdditionalNote,
            //   Createdby = AdminId <---- Need To Added Admin Id 
            };
            _dbContext?.Requestnotes.Add(reqNote);
            _dbContext?.SaveChanges();
        }else{
            var notesData = _dbContext.Requestnotes.FirstOrDefault(req=>req.Id == noteId);
            if(notesData!=null){
                notesData.Adminnotes = AdditionalNote;
                _dbContext.SaveChanges();
            }else{
                throw new Exception();
            }
        }
    }

    public short GetStatusOfRequest(int reqId){
        return _dbContext.Requests.FirstOrDefault(req => req.Id == reqId).Status;
    }
    public int? GetNoteIdFromRequestId(int reqId){
        return _dbContext.Requestnotes.FirstOrDefault(req => req.Requestid == reqId)?.Id;
    }
    public void ChangeStatusOfRequest(int reqId,short newStatus){
        Request RequestData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if(RequestData!=null){
            RequestData.Status = newStatus;
            _dbContext.SaveChanges();
        }else{
                throw new Exception();
        }
    }

    public void AddStatusLog(int reqId,short newStatus,short oldStatus,string reason,int? adminId,int? physicianId,int? transToPhyId=null){
        Requeststatuslog NewStatusLog = new(){
            Requestid = reqId,
            Status = newStatus,
            Oldstatus = oldStatus,
            Notes = reason,
            Transtophysicianid = transToPhyId
            // Adminid = adminId,
            // Physicianid = physicianId
        };
        _dbContext.Requeststatuslogs.Add(NewStatusLog);
        _dbContext.SaveChanges();
    }

    public async Task<IEnumerable<Region>> GetRegions(){
        return await _dbContext.Regions.ToListAsync();
    }
    public async Task<IEnumerable<Casetag>> GetCaseTag(){
        return await _dbContext.Casetags.ToListAsync();
    }

    public async Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId){
        var physicians = await _dbContext.Physicianregions
        .Where(phyReg => phyReg.Regionid == regionId)
        .Select(phyReg => phyReg.Physician)
        .ToListAsync();

        return physicians.Any() ? physicians : null;

    }

    public void AddPhysicianToRequest(int reqId,int transPhyId){
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if(reqData != null){
            reqData.Physicianid = transPhyId;
             _dbContext.SaveChanges();
        }else{
            throw new Exception();
        }
    }

    public Request GetSingleRequestDetails(int reqId){
        return _dbContext.Requests.Include(req => req.Requestclients).FirstOrDefault(req => req.Id == reqId);
    }

    public void SetBlockFieldRequest(int reqId){
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if(reqData != null){
            reqData.IsBlocked = true;
             _dbContext.SaveChanges();
        }else{
            throw new Exception();
        }
    }

    public void AddBlockRequest(Blockrequest newBlockReq){
        _dbContext.Blockrequests.Add(newBlockReq);
        _dbContext.SaveChanges();
    }

    public Request GetSingleRequest(int reqId){
        return _dbContext.Requests.Include(req => req.User).Include(req=>req.Requestclients).FirstOrDefault(req => req.Id == reqId);
    }

    public void DeleteDocument(int docId){
        Requestwisefile fileData =  _dbContext.Requestwisefiles.FirstOrDefault(doc => doc.Id == docId);
        if(fileData!=null){
                fileData.Isdeleted = true;
                _dbContext.SaveChanges();
        }
    }

    public IEnumerable<Healthprofessionaltype> GetAllProfessions(){
        return _dbContext.Healthprofessionaltypes.ToList();
    }
    public IEnumerable<Healthprofessional> GetBusinessByProfession(int professionId){
        return _dbContext.Healthprofessionals.Where(prof => prof.Profession == professionId);
    }
    public Healthprofessional GetBusinessDetails(int businessId){
        return _dbContext.Healthprofessionals.FirstOrDefault(prof => prof.Id == businessId);
    }

    public void AddOrderDetails(Orderdetail newOrder){
        _dbContext.Orderdetails.Add(newOrder);
        _dbContext.SaveChanges();
    }

}
