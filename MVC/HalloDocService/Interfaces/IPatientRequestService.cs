
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IPatientRequestService
{
    Aspnetuser GetUserByEmail(string email);
    Task ProcessPatientRequestAsync(PatientRequestViewModel patientView);
    Task<int> ProcessFamilyRequestAsync(FamilyRequestViewModel familyView);
    Task<int> ProcessConciergeRequestAsync(ConciergeRequestViewModel conciergeView);
    Task<int> ProcessBusinessRequestAsync(BusinessRequestViewModel businessView);
    
    void StoreActivationToken(int AspUserId , string token, DateTime expiry);

    IEnumerable<RegionList> GetAllRegions();

    
    
}
