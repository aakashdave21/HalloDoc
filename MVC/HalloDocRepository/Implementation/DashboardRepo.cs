
using HalloDocRepository.DataModels;
using HalloDocRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Implementation;
public class DashboardRepo : IDashboardRepo
{

    private readonly HalloDocContext _dbContext;

    public DashboardRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User GetUserByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Email == email);
    }
    public IEnumerable<Request> GetUserRequest(int userId)
    {
        var userRequests = _dbContext.Requests
                               .Where(data => data.Userid == userId)
                               .ToList();

        var requestIds = userRequests.Select(r => r.Id).ToList();

        var requestWithDocumentCount = _dbContext.Requests
                                              .Where(r => requestIds.Contains(r.Id))
                                              .GroupJoin(_dbContext.Requestwisefiles,
                                                          request => request.Id,
                                                          file => file.Requestid,
                                                          (request, files) => new
                                                          {
                                                              Request = request,
                                                              DocumentCount = files.Count()
                                                          })
                                              .ToList();

        foreach (var item in requestWithDocumentCount)
        {
            var request = item.Request;
            request.NoOfRequests = item.DocumentCount;
        }

        return userRequests;
        // return _dbContext.Requests.Where(data => data.Userid == userId).ToList();
    }

    public User GetUserData(int userId)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Id == userId);
    }

    public void EditUserProfile(int id, User userData)
    {
        User oldUserData = _dbContext.Users.FirstOrDefault(user => user.Id == id);

        if (oldUserData == null) return;

        oldUserData.Firstname = userData.Firstname == oldUserData.Firstname || userData.Firstname == null ? oldUserData.Firstname : userData.Firstname;
        oldUserData.Lastname = userData.Lastname == oldUserData.Lastname || userData.Lastname == null ? oldUserData.Lastname : userData.Lastname;
        oldUserData.City = userData.City == oldUserData.City || userData.City == null ? oldUserData.City : userData.City;
        oldUserData.State = userData.State == oldUserData.State || userData.State == null ? oldUserData.State : userData.State;
        oldUserData.Street = userData.Street == oldUserData.Street || userData.Street == null ? oldUserData.Street : userData.Street;
        oldUserData.Zipcode = userData.Zipcode == oldUserData.Zipcode || userData.Zipcode == null ? oldUserData.Zipcode : userData.Zipcode;
        oldUserData.Mobile = userData.Mobile == oldUserData.Mobile || userData.Mobile == null ? oldUserData.Mobile : userData.Mobile;
        oldUserData.Birthdate = userData.Birthdate == oldUserData.Birthdate || userData.Birthdate == null ? oldUserData.Birthdate : userData.Birthdate;
        oldUserData.Email = userData.Email == oldUserData.Email || userData.Email == null ? oldUserData.Email : userData.Email;

        _dbContext.SaveChanges();

    }

     public IEnumerable<Requestwisefile> GetAllRequestedDocuments(int reqId){
        return _dbContext.Requestwisefiles
                 .Where(rwf => rwf.Requestid == reqId && rwf.Isdeleted != true)          // Filter by request ID
                 .Include(rwf => rwf.Request)                   // Include related Request entity
                     .ThenInclude(req => req.Createduser)
                 .Include(rwf => rwf.Request.User)        // Include related User entity within Request
                 .Include(rq => rq.Request.Requestclients)
                 .ToList();
     }


}
