
using HalloDocRepository.DataModels;
using HalloDocRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Implementation;
public class PatientRequestRepo : IPatientRequestRepo
{

    private readonly HalloDocContext _dbContext;

    public PatientRequestRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Aspnetuser FindUserByEmail(string email){
        return _dbContext.Aspnetusers.FirstOrDefault(m => m.Email == email);
    }

    public User FindUserByEmailFromUser(string email){
        return _dbContext.Users.FirstOrDefault(m => m.Email == email);
    }

    public void AddRequestDataForExistedUser(Request requestData){
        _dbContext.Requests.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void AddPatientInfoForExistedUser(Requestclient requestData){
        _dbContext.Requestclients.Add(requestData);
        _dbContext.SaveChanges();
    }

    public void NewAspUserAdd(Aspnetuser requestData){
        _dbContext.Aspnetusers.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void NewUserAdd(User requestData){
        _dbContext.Users.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void NewRequestAdd(Request requestData){
        _dbContext.Requests.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void NewPatientAdd(Requestclient requestData){
        _dbContext.Requestclients.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void ConciergeDetailsAdd(Concierge requestData){
        _dbContext.Concierges.Add(requestData);
        _dbContext.SaveChanges();
    }
    public void RequestConciergeMappingAdd(Requestconcierge requestData){
        _dbContext.Requestconcierges.Add(requestData);
         _dbContext.SaveChanges();
    }
    public void AddDocumentDetails(Requestwisefile requestData){
        _dbContext.Requestwisefiles.Add(requestData);
        _dbContext.SaveChanges();
    }
     
}