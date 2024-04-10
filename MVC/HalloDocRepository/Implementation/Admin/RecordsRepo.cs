using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Data.Common;

namespace HalloDocRepository.Admin.Implementation;
public class RecordsRepo : IRecordsRepo
{
    private readonly HalloDocContext _dbContext;
    public RecordsRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (IEnumerable<Requestclient>, int) GetPatientHistory(Requestclient Parameters, int PageNum = 1, int PageSize = 5)
    {

        string firstname = Parameters.Firstname;
        string lastname = Parameters.Lastname;
        string email = Parameters.Email;
        string phone = Parameters.Phonenumber;

        var query = _dbContext.Requestclients.Include(client => client.Request).ThenInclude(req => req.User).AsQueryable();

        if (!string.IsNullOrEmpty(firstname)) query = query.Where(r => r.Firstname.ToLower().Contains(firstname));
        if (!string.IsNullOrEmpty(lastname)) query = query.Where(r => r.Lastname.ToLower().Contains(lastname));
        if (!string.IsNullOrEmpty(email)) query = query.Where(r => r.Email.ToLower().Contains(email));
        if (!string.IsNullOrEmpty(phone)) query = query.Where(r => r.Phonenumber.Contains(phone));

        int totalCount = query.Count();
        Console.WriteLine(totalCount);
        int skipCount = (PageNum - 1) * PageSize;
        query = query.Skip(skipCount).Take(PageSize);
        return (query.ToList(), totalCount);
    }

    public IEnumerable<Request> GetPatientRequest(int UserId, int RequestId)
    {
        if (UserId == 0)
        {
            return _dbContext.Requests.Include(req => req.Requestclients).Include(req => req.Physician).Include(req => req.Requeststatuslogs).Where(r => r.Id == RequestId && r.Isdeleted != true);
        }
        return _dbContext.Requests.Include(req => req.Requestclients).Include(req => req.Physician).Include(req => req.Requeststatuslogs).Where(req => req.Userid == UserId && req.Isdeleted != true);
    }

    public (IEnumerable<Request>, int) GetAllRecords(RecordsRepoView Parameters, int PageNum = 1, int PageSize = 5)
    {
        string? PatientName = Parameters.PatientName;
        string? PhysicianName = Parameters.PhysicianName;
        string? email = Parameters.Email;
        string? phone = Parameters.PhoneNumber;
        DateTime? InputFromDate = Parameters.InputFromDate.ToString() == "1/1/0001" ? null : Parameters.InputFromDate.ToDateTime(new TimeOnly(0, 0, 0));
        DateTime? InputToDate = Parameters.InputToDate.ToString() == "1/1/0001" ? null : Parameters.InputToDate.ToDateTime(new TimeOnly(0, 0, 0));
        int? InputRequestStatus = Parameters.InputRequestStatus;
        int? InputRequestType = Parameters.InputRequestType;

        IEnumerable<Request> query = _dbContext.Requests
            .Include(req => req.Requestclients)
            .Include(req => req.User)
            .Include(req => req.Physician)
            .Include(req => req.Requeststatuslogs)
            .Include(req => req.Requestnote);

        if (!string.IsNullOrEmpty(PatientName))
            query = query.Where(req => req.Requestclients.FirstOrDefault().Firstname.ToLower().Contains(PatientName.ToLower()));

        if (!string.IsNullOrEmpty(PhysicianName))
            query = query.Where(req => req.Physician != null && req.Physician.Firstname != null &&
                                req.Physician.Firstname.ToLower().Contains(PhysicianName.ToLower()));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(req => req.Requestclients.FirstOrDefault().Email.ToLower().Contains(email.ToLower()));

        if (!string.IsNullOrEmpty(phone))
            query = query.Where(req => req.Requestclients.FirstOrDefault().Phonenumber.Contains(phone));

        if (InputFromDate != null){
            query = query.Where(req => req.Accepteddate != null && req.Accepteddate >= InputFromDate);
        }

        if (InputToDate != null)
            query = query.Where(req =>  req.Accepteddate != null && req.Accepteddate <= InputToDate);

        if (InputRequestType != 0)
            query = query.Where(req => req.Requesttypeid == InputRequestType);

        int totalCount = query.Count();

        int skipCount = (PageNum - 1) * PageSize;
        query = query.Skip(skipCount).Take(PageSize);

        return (query.ToList(), totalCount);
    }
    
    public void DeleteRecord(int ReqId){
        Request? requestInfo = _dbContext.Requests.FirstOrDefault(req=>req.Id == ReqId);
        if(requestInfo!=null){
            _dbContext.Requests.Remove(requestInfo);
            _dbContext.SaveChanges();
            return;
        }
        throw new Exception();
    }
}

public class RecordsRepoView
{
    public string? PatientName { get; set; }
    public string? PhysicianName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int InputRequestStatus { get; set; }
    public int InputRequestType { get; set; }
    public DateOnly InputFromDate { get; set; }
    public DateOnly InputToDate { get; set; }
}