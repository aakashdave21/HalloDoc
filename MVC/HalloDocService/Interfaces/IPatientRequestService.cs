
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IPatientRequestService
{
    Aspnetuser GetUserByEmail(string email);
    void ProcessPatientRequestAsync(PatientRequestViewModel patientView);
    int ProcessFamilyRequestAsync(FamilyRequestViewModel familyView);
    int ProcessConciergeRequestAsync(ConciergeRequestViewModel conciergeView);
    int ProcessBusinessRequestAsync(BusinessRequestViewModel businessView);
    
    void StoreActivationToken(int AspUserId , string token, DateTime expiry);

    IEnumerable<RegionList> GetAllRegions();

    public string CreateConfirmation(int? State, string Firstname, string Lastname);

    
    
}
