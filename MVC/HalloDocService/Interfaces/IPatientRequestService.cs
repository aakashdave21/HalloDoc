
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IPatientRequestService
{
    Aspnetuser GetUserByEmail(string email);
    Task ProcessPatientRequestAsync(PatientRequestViewModel patientView);
    Task ProcessFamilyRequestAsync(FamilyRequestViewModel familyView);
    Task ProcessConciergeRequestAsync(ConciergeRequestViewModel conciergeView);
    Task ProcessBusinessRequestAsync(BusinessRequestViewModel businessView);
 
    
}
