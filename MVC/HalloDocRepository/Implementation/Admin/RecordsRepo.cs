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

    public (IEnumerable<Requestclient>,int) GetPatientHistory(Requestclient Parameters,int PageNum = 1,int PageSize = 5)
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
        return (query.ToList(),totalCount);
    }

    public IEnumerable<Request> GetPatientRequest(int UserId,int RequestId){
        if(UserId==0){
            return _dbContext.Requests.Include(req=>req.Requestclients).Include(req=>req.Physician).Where(r => r.Id == RequestId  && r.Isdeleted != true);
        }
        return _dbContext.Requests.Include(req=>req.Requestclients).Include(req=>req.Physician).Where(req => req.Userid == UserId && req.Isdeleted != true);
    }
}