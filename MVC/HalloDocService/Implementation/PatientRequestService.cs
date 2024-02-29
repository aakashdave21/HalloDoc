using HalloDocRepository.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Implementation;
public class PatientRequestService : IPatientRequestService
{
    private readonly IPatientRequestRepo _patientRequestRepo;
    private readonly IPatientLoginRepo _patientLoginRepo;

    // Patient Request Implementation
    public PatientRequestService(IPatientRequestRepo patientRequestRepo, IPatientLoginRepo patientLoginRepo)
    {
        _patientRequestRepo = patientRequestRepo;
        _patientLoginRepo = patientLoginRepo;
    }

    public async Task ProcessPatientRequestAsync(PatientRequestViewModel patientView)
    {

        var userEmail = _patientRequestRepo.FindUserByEmail(patientView.Email);

        if (userEmail != null)
        {
            await ProcessExistingUserRequestAsync(patientView);
        }
        else
        {
            await ProcessNewUserRequestAsync(patientView);
        }

    }
    private async Task ProcessExistingUserRequestAsync(PatientRequestViewModel viewRequest)
    {
        var existUserData = _patientRequestRepo.FindUserByEmailFromUser(viewRequest.Email);

        var newRequestForExistedUser = new Request
        {
            Userid = existUserData.Id,
            Symptoms = viewRequest.Symptoms,
            Roomnoofpatient = viewRequest.Roomnoofpatient,
            // Documentsofpatient = viewRequest.Documentsofpatient,
            Status = 1, // For Unassigned,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Requesttypeid = 1
        };
        _patientRequestRepo.AddRequestDataForExistedUser(newRequestForExistedUser);

        var patientInfo = new Requestclient
        {
            Requestid = newRequestForExistedUser.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Street = viewRequest.Street,
            Notes = viewRequest.Symptoms,
            City = viewRequest.City,
            State = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Strmonth = DateOnly.Parse(viewRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(viewRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(viewRequest.Birthdate).Day
        };
        _patientRequestRepo.AddPatientInfoForExistedUser(patientInfo);

        if (viewRequest.FilePath != null || viewRequest.FilePath.Length > 0)
        {
            var documentDetails = new Requestwisefile
            {
                Requestid = newRequestForExistedUser.Id,
                Filename = viewRequest.FilePath,
                Createddate = DateTime.Now,
            };
            _patientRequestRepo.AddDocumentDetails(documentDetails);
        }
    }

    private async Task ProcessNewUserRequestAsync(PatientRequestViewModel viewRequest)
    {
        string email = viewRequest.Email;
        string[] parts = email.Split('@');
        string userName = parts[0];

        var newUser = new Aspnetuser
        {
            Username = userName,
            Email = viewRequest.Email,
            Passwordhash = viewRequest.Passwordhash,
            Phonenumber = viewRequest.Mobile,
        };
        _patientRequestRepo.NewAspUserAdd(newUser);

        var newPatient = new User
        {
            Aspnetuserid = newUser.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Email = viewRequest.Email,
            Mobile = viewRequest.Mobile,
            Street = viewRequest.Street,
            City = viewRequest.City,
            State = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Birthdate = DateOnly.Parse(viewRequest.Birthdate),
            Createddate = DateTime.Now
        };
        _patientRequestRepo.NewUserAdd(newPatient);
        // _context.Users.Add(newPatient);
        // await _context.SaveChangesAsync();

        var newRequest = new Request
        {
            Userid = newPatient.Id,
            Symptoms = viewRequest.Symptoms,
            Roomnoofpatient = viewRequest.Roomnoofpatient,
            // Documentsofpatient = viewRequest.Documentsofpatient,
            Status = 1, // For Unassigned,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Requesttypeid = 1
        };
        _patientRequestRepo.NewRequestAdd(newRequest);

        var newPatientInfo = new Requestclient
        {
            Requestid = newRequest.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Street = viewRequest.Street,
            Notes = viewRequest.Symptoms,
            City = viewRequest.City,
            State = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Strmonth = DateOnly.Parse(viewRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(viewRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(viewRequest.Birthdate).Day
        };
        _patientRequestRepo.NewPatientAdd(newPatientInfo);

        if (viewRequest.FilePath != null || viewRequest.FilePath.Length > 0)
        {
            var documentDetails = new Requestwisefile
            {
                Requestid = newRequest.Id,
                Filename = viewRequest.FilePath,
                Createddate = DateTime.Now,
            };
            _patientRequestRepo.AddDocumentDetails(documentDetails);
        }
    }

    // Email Verification
    public Aspnetuser GetUserByEmail(string email)
    {
        return _patientLoginRepo.ValidateUser(email);
    }

    // Family Request Implementation
    public async Task ProcessFamilyRequestAsync(FamilyRequestViewModel familyView)
    {
        var userEmail = _patientRequestRepo.FindUserByEmail(familyView.Email);
        if (userEmail != null)
        {
            await ProcessExistingUserFamilyRequestAsync(familyView);
        }
        else
        {
            await ProcessNewUserFamilyRequestAsync(familyView);
            
        }
    }
    private async Task ProcessExistingUserFamilyRequestAsync(FamilyRequestViewModel familyRequest)
    {
        var existUserData = _patientRequestRepo.FindUserByEmailFromUser(familyRequest.Email);

        var newRequestForExistedUser = new Request
        {
            Userid = existUserData.Id,
            Symptoms = familyRequest.Symptoms,
            Roomnoofpatient = familyRequest.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = familyRequest.FamilyFirstname,
            Lastname = familyRequest.FamilyLastname,
            Phonenumber = familyRequest.FamilyPhonenumber,
            Email = familyRequest.FamilyEmail,
            Relationname = familyRequest.RelationWithPatient,
            Requesttypeid = 2 // For Family/Friends
        };
        _patientRequestRepo.AddRequestDataForExistedUser(newRequestForExistedUser);

        var patientInfo = new Requestclient
        {
            Requestid = newRequestForExistedUser.Id,
            Firstname = familyRequest.Firstname,
            Lastname = familyRequest.Lastname,
            Phonenumber = familyRequest.Mobile,
            Email = familyRequest.Email,
            Street = familyRequest.Street,
            Notes = familyRequest.Symptoms,
            City = familyRequest.City,
            State = familyRequest.State,
            Zipcode = familyRequest.Zipcode,
            Strmonth = DateOnly.Parse(familyRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(familyRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(familyRequest.Birthdate).Day
        };
        _patientRequestRepo.AddPatientInfoForExistedUser(patientInfo);

        if (familyRequest.FilePath != null || familyRequest.FilePath.Length > 0)
        {
            var documentDetails = new Requestwisefile
            {
                Requestid = newRequestForExistedUser.Id,
                Filename = familyRequest.FilePath,
                Createddate = DateTime.Now,
            };
            _patientRequestRepo.AddDocumentDetails(documentDetails);
        }
    }
    private async Task ProcessNewUserFamilyRequestAsync(FamilyRequestViewModel familyRequest)
    {
        // If user is not exists then new User Creation
        // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
        string email = familyRequest.Email;
        string[] parts = email.Split('@');
        string userName = parts[0];

        var newUser = new Aspnetuser
        {
            Username = userName,
            Email = familyRequest.Email,
            // Passwordhash = viewRequest.Passwordhash,
            Phonenumber = familyRequest.Mobile,
        };
        _patientRequestRepo.NewAspUserAdd(newUser);
        var newPatient = new User
        {
            Aspnetuserid = newUser.Id,
            Firstname = familyRequest.Firstname,
            Lastname = familyRequest.Lastname,
            Email = familyRequest.Email,
            Mobile = familyRequest.Mobile,
            Street = familyRequest.Street,
            City = familyRequest.City,
            State = familyRequest.State,
            Zipcode = familyRequest.Zipcode,
            Birthdate = DateOnly.Parse(familyRequest.Birthdate),
            Createddate = DateTime.Now
        };
        _patientRequestRepo.NewUserAdd(newPatient);
        var newRequest = new Request
        {
            Userid = newPatient.Id,
            Symptoms = familyRequest.Symptoms,
            Roomnoofpatient = familyRequest.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = familyRequest.FamilyFirstname,
            Lastname = familyRequest.FamilyLastname,
            Phonenumber = familyRequest.FamilyPhonenumber,
            Email = familyRequest.FamilyEmail,
            Relationname = familyRequest.RelationWithPatient,
            Requesttypeid = 2 // For Family/Friends
        };
        _patientRequestRepo.NewRequestAdd(newRequest);

        var newPatientInfo = new Requestclient
        {
            Requestid = newRequest.Id,
            Firstname = familyRequest.Firstname,
            Lastname = familyRequest.Lastname,
            Phonenumber = familyRequest.Mobile,
            Email = familyRequest.Email,
            Street = familyRequest.Street,
            City = familyRequest.City,
            State = familyRequest.State,
            Notes = familyRequest.Symptoms,
            Zipcode = familyRequest.Zipcode,
            Strmonth = DateOnly.Parse(familyRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(familyRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(familyRequest.Birthdate).Day
        };
        _patientRequestRepo.NewPatientAdd(newPatientInfo);

        if (familyRequest.FilePath != null || familyRequest.FilePath.Length > 0)
        {
            var documentDetails = new Requestwisefile
            {
                Requestid = newRequest.Id,
                Filename = familyRequest.FilePath,
                Createddate = DateTime.Now,
            };
            _patientRequestRepo.AddDocumentDetails(documentDetails);
        }
    }



    // Business Request Implementaion
    public async Task ProcessBusinessRequestAsync(BusinessRequestViewModel businessView)
    {
        var userEmail = _patientRequestRepo.FindUserByEmail(businessView.Email);
        if (userEmail != null)
        {
            await ProcessExistingUserBusinessRequestAsync(businessView);
        }
        else
        {
            await ProcessNewUserBusinessRequestAsync(businessView);
        }
    }

    private async Task ProcessExistingUserBusinessRequestAsync(BusinessRequestViewModel businessRequests)
    {
        var existUserData = _patientRequestRepo.FindUserByEmailFromUser(businessRequests.Email);

        var newRequestForExistedUser = new Request
        {
            Userid = existUserData.Id,
            Symptoms = businessRequests.Symptoms,
            Roomnoofpatient = businessRequests.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = businessRequests.BusinessFirstname,
            Lastname = businessRequests.BusinessLastname,
            Phonenumber = businessRequests.BusinessPhonenumber,
            Email = businessRequests.BusinessEmail,
            Relationname = "Business",
            PropertyName = businessRequests.PropertyName,
            Requesttypeid = 4 // For Business Partners
        };
        _patientRequestRepo.AddRequestDataForExistedUser(newRequestForExistedUser);

        var patientInfo = new Requestclient
        {
            Requestid = newRequestForExistedUser.Id,
            Firstname = businessRequests.Firstname,
            Lastname = businessRequests.Lastname,
            Phonenumber = businessRequests.Mobile,
            Email = businessRequests.Email,
            Street = businessRequests.Street,
            Notes = businessRequests.Symptoms,
            City = businessRequests.City,
            State = businessRequests.State,
            Zipcode = businessRequests.Zipcode,
            Strmonth = DateOnly.Parse(businessRequests.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(businessRequests.Birthdate).Year,
            Intdate = DateOnly.Parse(businessRequests.Birthdate).Day
        };
        _patientRequestRepo.AddPatientInfoForExistedUser(patientInfo);
    }
    private async Task ProcessNewUserBusinessRequestAsync(BusinessRequestViewModel businessRequests)
    {
        // If user is not exists then new User Creation
        // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
        string email = businessRequests.Email;
        string[] parts = email.Split('@');
        string userName = parts[0];

        var newUser = new Aspnetuser
        {
            Username = userName,
            Email = businessRequests.Email,
            // Passwordhash = viewRequest.Passwordhash,
            Phonenumber = businessRequests.Mobile,
        };
        _patientRequestRepo.NewAspUserAdd(newUser);
        var newPatient = new User
        {
            Aspnetuserid = newUser.Id,
            Firstname = businessRequests.Firstname,
            Lastname = businessRequests.Lastname,
            Email = businessRequests.Email,
            Mobile = businessRequests.Mobile,
            Street = businessRequests.Street,
            City = businessRequests.City,
            State = businessRequests.State,
            Zipcode = businessRequests.Zipcode,
            Birthdate = DateOnly.Parse(businessRequests.Birthdate),
            Createddate = DateTime.Now
        };
        _patientRequestRepo.NewUserAdd(newPatient);
        var newRequest = new Request
        {
            Userid = newPatient.Id,
            Symptoms = businessRequests.Symptoms,
            Roomnoofpatient = businessRequests.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = businessRequests.BusinessFirstname,
            Lastname = businessRequests.BusinessLastname,
            Phonenumber = businessRequests.BusinessPhonenumber,
            Email = businessRequests.BusinessEmail,
            Relationname = "Business",
            PropertyName = businessRequests.PropertyName,
            Requesttypeid = 4 // For Business Partners
        };
        _patientRequestRepo.NewRequestAdd(newRequest);

        var newPatientInfo = new Requestclient
        {
            Requestid = newRequest.Id,
            Firstname = businessRequests.Firstname,
            Lastname = businessRequests.Lastname,
            Phonenumber = businessRequests.Mobile,
            Email = businessRequests.Email,
            Street = businessRequests.Street,
            City = businessRequests.City,
            Notes = businessRequests.Symptoms,
            State = businessRequests.State,
            Zipcode = businessRequests.Zipcode,
            Strmonth = DateOnly.Parse(businessRequests.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(businessRequests.Birthdate).Year,
            Intdate = DateOnly.Parse(businessRequests.Birthdate).Day
        };
        _patientRequestRepo.NewPatientAdd(newPatientInfo);
    }

    // Concierge Request Implementatin
    public async Task ProcessConciergeRequestAsync(ConciergeRequestViewModel conciergeView)
    {
        var userEmail = _patientRequestRepo.FindUserByEmail(conciergeView.Email);
        if (userEmail != null)
        {
            await ProcessExistingUserConciergeRequestAsync(conciergeView);
        }
        else
        {
            await ProcessNewUserConciergeRequestAsync(conciergeView);
        }
    }

    private async Task ProcessExistingUserConciergeRequestAsync(ConciergeRequestViewModel conciergeRequest)
    {
        var existUserData = _patientRequestRepo.FindUserByEmailFromUser(conciergeRequest.Email);

        var newRequestForExistedUser = new Request
        {
            Userid = existUserData.Id,
            Symptoms = conciergeRequest.Symptoms,
            Roomnoofpatient = conciergeRequest.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = conciergeRequest.ConciergeFirstname,
            Lastname = conciergeRequest.ConciergeLastname,
            Phonenumber = conciergeRequest.ConciergePhonenumber,
            Email = conciergeRequest.ConciergeEmail,
            Relationname = "Concierge",
            PropertyName = conciergeRequest.PropertyName,
            Requesttypeid = 3 // For Family/Friends
        };
        _patientRequestRepo.AddRequestDataForExistedUser(newRequestForExistedUser);

        var patientInfo = new Requestclient
        {
            Requestid = newRequestForExistedUser.Id,
            Firstname = conciergeRequest.Firstname,
            Lastname = conciergeRequest.Lastname,
            Phonenumber = conciergeRequest.Mobile,
            Email = conciergeRequest.Email,
            Notes = conciergeRequest.Symptoms,
            Strmonth = DateOnly.Parse(conciergeRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(conciergeRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(conciergeRequest.Birthdate).Day
        };
        _patientRequestRepo.AddPatientInfoForExistedUser(patientInfo);

        var newConcierge = new Concierge
        {
            Conciergename = conciergeRequest.Firstname,
            Street = conciergeRequest.ConciergeStreet,
            City = conciergeRequest.ConciergeCity,
            State = conciergeRequest.ConciergeState,
            Zipcode = conciergeRequest.ConciergeZipcode
        };
        _patientRequestRepo.ConciergeDetailsAdd(newConcierge);

        var new_request_concierge = new Requestconcierge
        {
            Requestid = newRequestForExistedUser.Id,
            Conciergeid = newConcierge.Id
        };
        _patientRequestRepo.RequestConciergeMappingAdd(new_request_concierge);
    }
    private async Task ProcessNewUserConciergeRequestAsync(ConciergeRequestViewModel conciergeRequest)
    {
        // If user is not exists then new User Creation
        // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
        string email = conciergeRequest.Email;
        string[] parts = email.Split('@');
        string userName = parts[0];

        var newUser = new Aspnetuser
        {
            Username = userName,
            Email = conciergeRequest.Email,
            // Passwordhash = viewRequest.Passwordhash,
            Phonenumber = conciergeRequest.Mobile,
        };
        _patientRequestRepo.NewAspUserAdd(newUser);
        var newPatient = new User
        {
            Aspnetuserid = newUser.Id,
            Firstname = conciergeRequest.Firstname,
            Lastname = conciergeRequest.Lastname,
            Email = conciergeRequest.Email,
            Mobile = conciergeRequest.Mobile,
            Birthdate = DateOnly.Parse(conciergeRequest.Birthdate),
            Createddate = DateTime.Now
        };
        _patientRequestRepo.NewUserAdd(newPatient);

        var newRequest = new Request
        {
            Userid = newPatient.Id,
            Symptoms = conciergeRequest.Symptoms,
            Roomnoofpatient = conciergeRequest.Roomnoofpatient,
            Status = 1, // For Unassigned,
            Firstname = conciergeRequest.ConciergeFirstname,
            Lastname = conciergeRequest.ConciergeLastname,
            Phonenumber = conciergeRequest.ConciergePhonenumber,
            Email = conciergeRequest.ConciergeEmail,
            Relationname = "Concierge",
            PropertyName = conciergeRequest.PropertyName,
            Requesttypeid = 3 // For Family/Friends
        };
        _patientRequestRepo.NewRequestAdd(newRequest);

        var newPatientInfo = new Requestclient
        {
            Requestid = newRequest.Id,
            Firstname = conciergeRequest.Firstname,
            Lastname = conciergeRequest.Lastname,
            Phonenumber = conciergeRequest.Mobile,
            Notes = conciergeRequest.Symptoms,
            Email = conciergeRequest.Email,
            Strmonth = DateOnly.Parse(conciergeRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(conciergeRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(conciergeRequest.Birthdate).Day
        };
        _patientRequestRepo.NewPatientAdd(newPatientInfo);

        var newConciergeForNewUser = new Concierge
        {
            Conciergename = conciergeRequest.Firstname,
            Street = conciergeRequest.ConciergeStreet,
            City = conciergeRequest.ConciergeCity,
            State = conciergeRequest.ConciergeState,
            Zipcode = conciergeRequest.ConciergeZipcode
        };
        _patientRequestRepo.ConciergeDetailsAdd(newConciergeForNewUser);

        var new_request_concierge_for_newcustomer = new Requestconcierge
        {
            Requestid = newRequest.Id,
            Conciergeid = newConciergeForNewUser.Id
        };
        _patientRequestRepo.RequestConciergeMappingAdd(new_request_concierge_for_newcustomer);
    }
}


