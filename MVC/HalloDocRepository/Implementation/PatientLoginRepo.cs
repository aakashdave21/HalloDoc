
using HalloDocRepository.DataModels;
using HalloDocRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.Implementation;
public class PatientLoginRepo : IPatientLoginRepo
{

    private readonly HalloDocContext _dbContext;

    public PatientLoginRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Aspnetuser ValidateUser(string email)
    {
        return _dbContext.Aspnetusers.FirstOrDefault( user => user.Email == email);
    }

    public Aspnetuser userDetailsFromUserName(string username){
        return _dbContext.Aspnetusers.FirstOrDefault(q=> q.Username == username);
    }

    public User UserDetailsFetch(string email){
        return _dbContext.Users.FirstOrDefault(q => q.Email == email);
    }

    public void StoreResetToken(int AspUserId, string token, DateTime expiry){
        var userData = _dbContext.Aspnetusers.FirstOrDefault( user => user.Id == AspUserId);
        userData.ResetToken = token;
        userData.ResetExpiration = expiry;
        _dbContext.SaveChanges();
    }

    public Aspnetuser? GetResetTokenExpiry(int AspUserId, string token){
        return _dbContext.Aspnetusers.FirstOrDefault(user => user.Id==AspUserId);

    }
}