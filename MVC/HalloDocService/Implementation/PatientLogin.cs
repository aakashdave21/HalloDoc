using HalloDocRepository.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using AdminTable = HalloDocRepository.DataModels.Admin;


namespace HalloDocService.Implementation;
public class PatientLogin : IPatientLogin
{
    private readonly IPatientLoginRepo _userRepository;

    public PatientLogin(IPatientLoginRepo userRepository)
    {
        _userRepository = userRepository;
    }

    public Aspnetuser ValidateUser(UserLoginViewModel userLoginVm)
    {
        //Convert view model => DataModel Here
        Aspnetuser isValidUser = _userRepository.ValidateUser(userLoginVm.Email);
        //var emailList = users.Select(x => x.Email);
        return isValidUser;
    }

    public bool VerifyPassword(string userPassword,string storedHashPassword){
        // Check here using Hashing Method
        var isValid = userPassword==storedHashPassword ? true : false;
        return isValid;
    }

    public Aspnetuser FindUserFromUsername(UserResetPasswordViewModel user){
        Aspnetuser? userDetailsFromUsername = _userRepository.userDetailsFromUserName(user.Email);
        return userDetailsFromUsername; 
    }

    public User UserDetailsFetch(string email){
        return _userRepository.UserDetailsFetch(email);
    }
    public AdminTable AdminDetailsFetch(string email){
        return _userRepository.AdminDetailsFetch(email);
    }
    public Physician ProviderDetailsFetch(string email){
        return _userRepository.ProviderDetailsFetch(email);
    }

    public void StoreResetToken(int AspUserId, string token, DateTime expiry){
        _userRepository.StoreResetToken(AspUserId,token,expiry);
    }

    public Aspnetuser? GetResetTokenExpiry(int AspUserId, string token){
        return _userRepository.GetResetTokenExpiry(AspUserId, token);
    }

     public void UpdatePassword(int AspUserId, string password){
        _userRepository.UpdatePassword(AspUserId,password);
    }

    public string GetAspUserEmail(int userId){
        return _userRepository.GetAspUserEmail(userId);
    }


}
