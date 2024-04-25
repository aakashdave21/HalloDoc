
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using HalloDocRepository.Enums;
using HalloDocRepository.CustomModels;


namespace HalloDocRepository.Admin.Implementation;
public class AdminDashboardRepo : IAdminDashboardRepo
{

    private readonly HalloDocContext _dbContext;

    public AdminDashboardRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (IEnumerable<Request> requests, int totalCount) GetDashboardRequests(DashboardRequestQuery Params)
    {
        IQueryable<Request> query = _dbContext.Requests
            .Include(req => req.Requestclients)
            .Include(req => req.Physician)
            .Include(req => req.Encounterform);
        query = Params.Status switch
        {
            "new" => query.Where(req => req.Status == (short)RequestStatusEnum.Unassigned),
            "pending" => query.Where(req => req.Status == (short)RequestStatusEnum.Accepted),
            "active" => query.Where(req => (req.Status == (short)RequestStatusEnum.MdRequest || req.Status == (short)RequestStatusEnum.MDONSite) && req.Accepteddate != null),
            "conclude" => query.Where(req => req.Status == (short)RequestStatusEnum.Conclude),
            "close" => query.Where(req => req.Status == (short)RequestStatusEnum.Cancelled || req.Status == (short)RequestStatusEnum.CancelledByPatient || req.Status == (short)RequestStatusEnum.Closed),
            "unpaid" => query.Where(req => req.Status == (short)RequestStatusEnum.Unpaid),
            _ => query.Where(req => req.Status == (short)RequestStatusEnum.Unassigned),
        };
        if (Params.RequestTypeId > 0) query = query.Where(req => req.Requesttypeid == Params.RequestTypeId);
        if (Params.Region != 0) query = query.Where(req => req.Requestclients.FirstOrDefault().Regionid == Params.Region);
        if (!string.IsNullOrWhiteSpace(Params.SearchBy)) query = query.Where(req => req.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(Params.SearchBy)));
        query = query.OrderByDescending(req => req.Createdat);
        int totalCount = query.Count();
        query = query.Skip((Params.PageNumber - 1) * Params.PageSize)
                    .Take(Params.PageSize);
        return (query.ToList(), totalCount);
    }
 
    public Dictionary<string, int> CountRequestByType()
    {
        Dictionary<string, int>? CountRequestRow = new();

        CountRequestRow = _dbContext.Requests
        .GroupBy(req => true)
        .Select(group => new
        {
            NewCount = group.Count(req => req.Status == (short)RequestStatusEnum.Unassigned),
            PendingCount = group.Count(req => req.Status == (short)RequestStatusEnum.Accepted),
            ActiveCount = group.Count(req => (req.Status == (short)RequestStatusEnum.MdRequest || req.Status == (short)RequestStatusEnum.MDONSite) && req.Accepteddate != null),
            ConcludeCount = group.Count(req => req.Status == (short)RequestStatusEnum.Conclude),
            CloseCount = group.Count(req => req.Status == (short)RequestStatusEnum.Cancelled || req.Status == (short)RequestStatusEnum.CancelledByPatient || req.Status == (short)RequestStatusEnum.Closed),
            UnpaidCount = group.Count(req => req.Status == (short)RequestStatusEnum.Unpaid)
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

    public Request GetViewCaseDetails(int id)
    {
        return _dbContext.Requests.Include(item => item.Requestclients)
                                        .ThenInclude(req => req.Region)
                                    .Include(req => req.Requesttype)
                                    .FirstOrDefault(req => req.Id == id);
    }

    public Requestnote GetViewNotesDetails(int reqId)
    {
        return _dbContext.Requestnotes.FirstOrDefault(req => req.Requestid == reqId);
    }
    public Requestclient GetPatientNoteDetails(int reqId)
    {
        return _dbContext.Requestclients.FirstOrDefault(req => req.Requestid == reqId);
    }
    public IQueryable<Requeststatuslog> GetAllCancelNotes(int reqId)
    {
        var cancelNotes = _dbContext.Requeststatuslogs
        .Where(log => log.Requestid == reqId && (log.Status == 7 || log.Status == 3 || log.Status == 2));

        return cancelNotes;

    }
    public void SaveAdditionalNotes(string AdditionalNote, int noteId, int reqId, int reqType = 1)
    {
        if (noteId == 0)
        {
            // We have to add new Records for that
            Requestnote reqNote = new()
            {
                Requestid = reqId,
                Adminnotes = reqType == 1 ? AdditionalNote : "",
                Physiciannotes = reqType == 2 ? AdditionalNote : ""
                //   Createdby = AdminId <---- Need To Added Admin Id 
            };
            _dbContext?.Requestnotes.Add(reqNote);
            _dbContext?.SaveChanges();
        }
        else
        {
            var notesData = _dbContext.Requestnotes.FirstOrDefault(req => req.Id == noteId);
            if (notesData != null)
            {
                if (reqType == 1)
                {
                    notesData.Adminnotes = AdditionalNote;
                }
                else if (reqType == 2)
                {
                    notesData.Physiciannotes = AdditionalNote;
                }
                _dbContext.SaveChanges();
            }
            else
            {
                throw new RecordNotFoundException();
            }
        }
    }

    public short GetStatusOfRequest(int reqId)
    {
        return _dbContext.Requests.FirstOrDefault(req => req.Id == reqId).Status;
    }
    public int? GetNoteIdFromRequestId(int reqId)
    {
        return _dbContext.Requestnotes.FirstOrDefault(req => req.Requestid == reqId)?.Id;
    }
    public void ChangeStatusOfRequest(int reqId, short newStatus)
    {
        Request RequestData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (RequestData != null)
        {
            RequestData.Status = newStatus;
            _dbContext.SaveChanges();
            return;
        }
        throw new RecordNotFoundException();
    }

    public void AddStatusLog(int reqId, short newStatus, short oldStatus, string reason, int? adminId = null, int? physicianId = null, int? transToPhyId = null, bool TransToAdmin = false)
    {
        Requeststatuslog NewStatusLog = new()
        {
            Requestid = reqId,
            Status = newStatus,
            Oldstatus = oldStatus,
            Notes = reason,
            Transtophysicianid = transToPhyId,
            Adminid = adminId,
            Physicianid = physicianId,
            Transtoadmin = TransToAdmin
        };
        _dbContext.Requeststatuslogs.Add(NewStatusLog);
        _dbContext.SaveChanges();
    }

    public List<Region> GetRegions()
    {
        return _dbContext.Regions.Include(reg => reg.Admins).ToList();
    }
    public async Task<IEnumerable<Casetag>> GetCaseTag()
    {
        return await _dbContext.Casetags.ToListAsync();
    }

    public async Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId)
    {
        var physicians = await _dbContext.Physicianregions
        .Where(phyReg => phyReg.Regionid == regionId)
        .Select(phyReg => phyReg.Physician)
        .ToListAsync();

        return physicians.Any() ? physicians : null;

    }

    public void AddPhysicianToRequest(int reqId, int? transPhyId = null)
    {
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.Physicianid = transPhyId;
            _dbContext.SaveChanges();
        }
        else
        {
            throw new RecordNotFoundException();
        }
    }

    public Request GetSingleRequestDetails(int reqId)
    {
        return _dbContext.Requests
                      .Include(req => req.Requestclients)
                      .Include(req => req.Physician)
                      .FirstOrDefault(req => req.Id == reqId);
    }

    public void SetBlockFieldRequest(int reqId)
    {
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.IsBlocked = true;
            _dbContext.SaveChanges();
            return;
        }
        throw new RecordNotFoundException();

    }

    public void AddBlockRequest(Blockrequest newBlockReq)
    {

        _dbContext.Blockrequests.Add(newBlockReq);
        _dbContext.SaveChanges();
    }

    public Request GetSingleRequest(int reqId)
    {
        return _dbContext.Requests.Include(req => req.User).Include(req => req.Requestclients).Include(req => req.Physician).FirstOrDefault(req => req.Id == reqId);
    }

    public void DeleteDocument(int docId)
    {
        Requestwisefile fileData = _dbContext.Requestwisefiles.FirstOrDefault(doc => doc.Id == docId);
        if (fileData != null)
        {
            fileData.Isdeleted = true;
            _dbContext.SaveChanges();
        }
    }

    public IEnumerable<Healthprofessionaltype> GetAllProfessions()
    {
        return _dbContext.Healthprofessionaltypes.ToList();
    }
    public IEnumerable<Healthprofessional> GetBusinessByProfession(int professionId)
    {
        return _dbContext.Healthprofessionals.Where(prof => prof.Profession == professionId);
    }
    public Healthprofessional GetBusinessDetails(int businessId)
    {
        return _dbContext.Healthprofessionals.FirstOrDefault(prof => prof.Id == businessId);
    }

    public void AddOrderDetails(Orderdetail newOrder)
    {
        _dbContext.Orderdetails.Add(newOrder);
        _dbContext.SaveChanges();
    }

    public void StoreAcceptToken(int reqId, string token, DateTime expirationTime)
    {
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.Updatedat = DateTime.Now;
            reqData.AcceptToken = token;
            reqData.AcceptExpiry = expirationTime;
            _dbContext.SaveChanges();
        }
    }

    public void AgreementAccept(int reqId)
    {
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.Updatedat = DateTime.Now;
            reqData.Status = (short)RequestStatusEnum.MdRequest; // Accept request
            reqData.Accepteddate = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
    public void AgreementReject(int reqId)
    {
        Request reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.Updatedat = DateTime.Now;
            reqData.Status = (short)RequestStatusEnum.CancelledByPatient; // Closed By Patient
            reqData.Accepteddate = null;
            _dbContext.SaveChanges();
        }
    }

    public void EditPatientInfo(string Email, string Phone, int patientId, int requestId)
    {
        Requestclient patientInfo = _dbContext.Requestclients.FirstOrDefault(patient => patient.Id == patientId);
        if (patientInfo != null)
        {
            patientInfo.Email = Email;
            patientInfo.Phonenumber = Phone;
            _dbContext.SaveChanges();
        }
    }

    public Request GetEncounterDetails(int reqId)
    {
        return _dbContext.Requests.Include(req => req.Encounterform).Include(req => req.Requestclients).Include(req => req.User).FirstOrDefault(req => req.Id == reqId);
    }

    public void SubmitEncounter(Encounterform encounterform)
    {
        if (encounterform.Id == null || encounterform.Id == 0)
        {
            _dbContext.Encounterforms.Add(encounterform);

        }
        else
        {
            Encounterform encounter = _dbContext.Encounterforms.FirstOrDefault(enc => enc.Id == encounterform.Id);
            if (encounter != null)
            {
                encounter.Historyofpresentillness = encounterform.Historyofpresentillness;
                encounter.Medicalhistory = encounterform.Medicalhistory;
                encounter.Medications = encounterform.Medications;
                encounter.Allergies = encounterform.Allergies;
                encounter.Temperature = encounterform.Temperature;
                encounter.Heartrate = encounterform.Heartrate;
                encounter.Respiratoryrate = encounterform.Respiratoryrate;
                encounter.Bloodpressure = encounterform.Bloodpressure;
                encounter.O2 = encounterform.O2;
                encounter.Pain = encounterform.Pain;
                encounter.Heent = encounterform.Heent;
                encounter.Cv = encounterform.Cv;
                encounter.Chest = encounterform.Chest;
                encounter.Abd = encounterform.Abd;
                encounter.Extr = encounterform.Extr;
                encounter.Skin = encounterform.Skin;
                encounter.Neuro = encounterform.Neuro;
                encounter.Other = encounterform.Other;
                encounter.Diagnosis = encounterform.Diagnosis;
                encounter.Treatmentplan = encounterform.Treatmentplan;
                encounter.Medicationdispensed = encounterform.Medicationdispensed;
                encounter.Procedures = encounterform.Procedures;
                encounter.Followup = encounterform.Followup;
                encounter.Updatedat = DateTime.Now;
            }

        }
        _dbContext.SaveChanges();
    }

    public void AddCallType(short callType, int reqId)
    {
        var reqData = _dbContext.Requests.FirstOrDefault(req => req.Id == reqId);
        if (reqData != null)
        {
            reqData.Calltype = callType;
            reqData.Updatedat = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }

    public IEnumerable<Request> FetchAllRequest()
    {
        return _dbContext.Requests.Include(req => req.Requestclients).Include(req => req.Physician).Include(req => req.User);
    }

    public AdminTable? GetAdminFromAsp(int AspId)
    {
        AdminTable? adminInfo = _dbContext.Admins.FirstOrDefault(req => req.Aspnetuserid == AspId);
        if (adminInfo != null)
        {
            return adminInfo;
        }
        return null;
    }

    public IEnumerable<Rolemenu>? GetUserRoles(int AspUserId)
    {
        int? roleId = _dbContext?.Admins?.FirstOrDefault(user => user.Aspnetuserid == AspUserId)?.Roleid ?? 0;
        if (roleId != 0)
        {
            return _dbContext?.Rolemenus.Include(rm => rm.Menu).Include(rm => rm.Role).Where(rm => rm.Roleid == roleId);
        }
        return null;
    }

}
