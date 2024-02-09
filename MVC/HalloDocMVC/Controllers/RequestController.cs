using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocMVC.Data;
using HalloDocMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HalloDocMVC.Controllers;

public class RequestController : Controller
{
    private readonly HalloDocContext _context;

    public RequestController(HalloDocContext context)
    {
        _context = context;
    }


    // Patient Request Page
    public IActionResult Patient()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatientPost(PatientRequestViewModel viewRequest)
    {
          Console.WriteLine("ModelState errors:");
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {

                    Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                }
            }

        if (!ModelState.IsValid)
        {   
            TempData["error"] = "Something went wrong! Please enter your details correct";
            return View(nameof(Patient), viewRequest); // Return the view with validation errors
        }
       

        try
        {
            var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == viewRequest.Email);
            
            // IF user already exists then enter only new request
            if (userEmail != null)
            {
                var existUserData = await _context.Users.FirstOrDefaultAsync(m => m.Email == viewRequest.Email);
                var newRequestForExistedUser = new Request
                {
                    Userid = existUserData.Id,
                    Symptoms = viewRequest.Symptoms,
                    Roomnoofpatient = viewRequest.Roomnoofpatient,
                    Documentsofpatient = viewRequest.Documentsofpatient,
                    Status = 1, // For Unassigned,
                    Firstname = viewRequest.Firstname,
                    Lastname = viewRequest.Lastname,
                    Phonenumber = viewRequest.Mobile,
                    Email = viewRequest.Email,
                    Requesttypeid = 1
                };
                _context.Requests.Add(newRequestForExistedUser);
                await _context.SaveChangesAsync();

                var patientInfo = new Requestclient
                {
                    Requestid = newRequestForExistedUser.Id,
                    Firstname = viewRequest.Firstname,
                    Lastname = viewRequest.Lastname,
                    Phonenumber = viewRequest.Mobile,
                    Email = viewRequest.Email,
                    Street = viewRequest.Street,
                    City = viewRequest.City,
                    State = viewRequest.State,
                    Zipcode = viewRequest.Zipcode,
                    Strmonth = viewRequest.Birthdate.Value.ToString("MMMM"),
                    Intyear = viewRequest.Birthdate.Value.Year,
                    Intdate = viewRequest.Birthdate.Value.Day

                };
                _context.Requestclients.Add(patientInfo);
                await _context.SaveChangesAsync();

                TempData["success"] = "Request Submitted Successfully";
                return RedirectToAction("Index", "PatientLogin");
            }
            
                // If User Not Exists then create new request 
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
                var newUserDetails = _context.Aspnetusers.Add(newUser);
                await _context.SaveChangesAsync();

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
                    Birthdate = viewRequest.Birthdate,
                    Createddate = DateTime.Now
                };
                _context.Users.Add(newPatient);
                await _context.SaveChangesAsync();

                var newRequest = new Request
                {
                    Userid = newPatient.Id,
                    Symptoms = viewRequest.Symptoms,
                    Roomnoofpatient = viewRequest.Roomnoofpatient,
                    Documentsofpatient = viewRequest.Documentsofpatient,
                    Status = 1, // For Unassigned,
                    Firstname = viewRequest.Firstname,
                    Lastname = viewRequest.Lastname,
                    Phonenumber = viewRequest.Mobile,
                    Email = viewRequest.Email,
                    Requesttypeid = 1
                };
                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                 var newPatientInfo = new Requestclient
                {
                    Requestid = newRequest.Id,
                    Firstname = viewRequest.Firstname,
                    Lastname = viewRequest.Lastname,
                    Phonenumber = viewRequest.Mobile,
                    Email = viewRequest.Email,
                    Street = viewRequest.Street,
                    City = viewRequest.City,
                    State = viewRequest.State,
                    Zipcode = viewRequest.Zipcode
                };
                _context.Requestclients.Add(newPatientInfo);
                await _context.SaveChangesAsync();

                TempData["success"] = "Request Submitted Successfully";
                return RedirectToAction("Index", "PatientLogin");

        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            ModelState.AddModelError("", "An error occurred while saving the data.");
            TempData["error"] = "An error occurred while saving the data.";
            
            return View(nameof(Patient), viewRequest);
        }
    }



    // EMAIL VERIFICATION AT REQUEST PAGE
    [HttpPost]
    public async Task<IActionResult> VerifyEmail(string email)
    {
        try
        {
            var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
            bool isValid = false;
            if (userEmail != null)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return Json(new { isValid = isValid });
        }
        catch (Exception e)
        {
            TempData["error"] = "An error occurred while Verifying the Email.";
            return View(nameof(Patient));
        }
    }


    // Family Request Page
    public IActionResult Family()
    {
        return View();
    }
    public async Task<IActionResult> FamilyPost(FamilyRequestViewModel familyRequest){

             Console.WriteLine(familyRequest);

            Console.WriteLine("ModelState errors:");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {

                        Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                    }
                }
            if (!ModelState.IsValid)
            {   
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Family), familyRequest); // Return the view with validation errors
            }

            try{
                var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == familyRequest.Email);
                if(userEmail != null){
                    var existUserData = await _context.Users.FirstOrDefaultAsync(m => m.Email == familyRequest.Email);
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
                    _context.Requests.Add(newRequestForExistedUser);
                    await _context.SaveChangesAsync();

                    var patientInfo = new Requestclient
                    {
                        Requestid = newRequestForExistedUser.Id,
                        Firstname = familyRequest.Firstname,
                        Lastname = familyRequest.Lastname,
                        Phonenumber = familyRequest.Mobile,
                        Email = familyRequest.Email,
                        Street = familyRequest.Street,
                        City = familyRequest.City,
                        State = familyRequest.State,
                        Zipcode = familyRequest.Zipcode,
                        Strmonth = familyRequest.Birthdate.Value.ToString("MMMM"),
                        Intyear = familyRequest.Birthdate.Value.Year,
                        Intdate = familyRequest.Birthdate.Value.Day
                    };
                    _context.Requestclients.Add(patientInfo);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Request Submitted Successfully";
                    return RedirectToAction("Index", "PatientLogin");
                }


                // If user is not exists then new User Creation
                // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
                var newRequest = new Request
                    {
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
                    _context.Requests.Add(newRequest);
                    await _context.SaveChangesAsync();

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
                        Zipcode = familyRequest.Zipcode,
                        Strmonth = familyRequest.Birthdate.Value.ToString("MMMM"),
                        Intyear = familyRequest.Birthdate.Value.Year,
                        Intdate = familyRequest.Birthdate.Value.Day
                };
                _context.Requestclients.Add(newPatientInfo);
                await _context.SaveChangesAsync();
                
                TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                return RedirectToAction("Index", "PatientLogin");
            }
            catch{
                // Log the exception or handle it accordingly
                ModelState.AddModelError("", "An error occurred while saving the data.");
                TempData["error"] = "An error occurred while saving the data.";
                
                return View(nameof(Family), familyRequest);
            }
    }

    public IActionResult Concierge(){
        return View();
    }

    public async Task<IActionResult> ConciergePost(ConciergeRequestViewModel conciergeRequest){
         Console.WriteLine(conciergeRequest);

            Console.WriteLine("ModelState errors:");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {

                        Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                    }
                }
            if (!ModelState.IsValid)
            {   
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Concierge), conciergeRequest); // Return the view with validation errors
            }
            try{
                var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == conciergeRequest.Email);

                // If User is Already Exits then Do it
                if(userEmail != null){
                    var existUserData = await _context.Users.FirstOrDefaultAsync(m => m.Email == conciergeRequest.Email);
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
                    _context.Requests.Add(newRequestForExistedUser);
                    await _context.SaveChangesAsync();

                    var patientInfo = new Requestclient
                    {
                        Requestid = newRequestForExistedUser.Id,
                        Firstname = conciergeRequest.Firstname,
                        Lastname = conciergeRequest.Lastname,
                        Phonenumber = conciergeRequest.Mobile,
                        Email = conciergeRequest.Email,
                        Strmonth = conciergeRequest.Birthdate.Value.ToString("MMMM"),
                        Intyear = conciergeRequest.Birthdate.Value.Year,
                        Intdate = conciergeRequest.Birthdate.Value.Day
                    };
                    _context.Requestclients.Add(patientInfo);
                    await _context.SaveChangesAsync();

                    var newConcierge = new Concierge
                    {
                        Conciergename = conciergeRequest.Firstname,
                        Street = conciergeRequest.ConciergeStreet,
                        City = conciergeRequest.ConciergeCity,
                        State = conciergeRequest.ConciergeState,
                        Zipcode = conciergeRequest.ConciergeZipcode
                    };
                    _context.Concierges.Add(newConcierge);
                    await _context.SaveChangesAsync();

                    var new_request_concierge = new Requestconcierge
                    {
                        Requestid = newRequestForExistedUser.Id,
                        Conciergeid = newConcierge.Id
                    };
                    _context.Requestconcierges.Add(new_request_concierge);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Request Submitted Successfully";
                    return RedirectToAction("Index", "PatientLogin");
                }


                // If user is not exists then new User Creation
                // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
                var newRequest = new Request
                    {
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
                    _context.Requests.Add(newRequest);
                    await _context.SaveChangesAsync();

                    var patientInfoForNewUser = new Requestclient
                    {
                        Requestid = newRequest.Id,
                        Firstname = conciergeRequest.Firstname,
                        Lastname = conciergeRequest.Lastname,
                        Phonenumber = conciergeRequest.Mobile,
                        Email = conciergeRequest.Email,
                        Strmonth = conciergeRequest.Birthdate.Value.ToString("MMMM"),
                        Intyear = conciergeRequest.Birthdate.Value.Year,
                        Intdate = conciergeRequest.Birthdate.Value.Day
                    };
                    _context.Requestclients.Add(patientInfoForNewUser);
                    await _context.SaveChangesAsync();

                    var newConciergeForNewUser = new Concierge
                    {
                        Conciergename = conciergeRequest.Firstname,
                        Street = conciergeRequest.ConciergeStreet,
                        City = conciergeRequest.ConciergeCity,
                        State = conciergeRequest.ConciergeState,
                        Zipcode = conciergeRequest.ConciergeZipcode
                    };
                    _context.Concierges.Add(newConciergeForNewUser);
                    await _context.SaveChangesAsync();

                    var new_request_concierge_for_newcustomer = new Requestconcierge
                    {
                        Requestid = newRequest.Id,
                        Conciergeid = newConciergeForNewUser.Id
                    };
                    _context.Requestconcierges.Add(new_request_concierge_for_newcustomer);
                    await _context.SaveChangesAsync();
                
                TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                return RedirectToAction("Index", "PatientLogin");
            }
            catch{
                // Log the exception or handle it accordingly
                ModelState.AddModelError("", "An error occurred while saving the data.");
                TempData["error"] = "An error occurred while saving the data.";
                
                return View(nameof(Concierge), conciergeRequest);
            }
    }



    // Business Request
    public IActionResult Business(){
        return View();
    }
        public async Task<IActionResult> BusinessPost(BusinessRequestViewModel businessRequests){

             Console.WriteLine(businessRequests);

            Console.WriteLine("ModelState errors:");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {

                        Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                    }
                }
            if (!ModelState.IsValid)
            {   
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Business), businessRequests); // Return the view with validation errors
            }

            try{
                var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == businessRequests.Email);
                if(userEmail != null){
                    var existUserData = await _context.Users.FirstOrDefaultAsync(m => m.Email == businessRequests.Email);
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
                        Requesttypeid = 3 // For Business Partners
                    };
                    _context.Requests.Add(newRequestForExistedUser);
                    await _context.SaveChangesAsync();

                    var patientInfo = new Requestclient
                    {
                        Requestid = newRequestForExistedUser.Id,
                        Firstname = businessRequests.Firstname,
                        Lastname = businessRequests.Lastname,
                        Phonenumber = businessRequests.Mobile,
                        Email = businessRequests.Email,
                        Street = businessRequests.Street,
                        City = businessRequests.City,
                        State = businessRequests.State,
                        Zipcode = businessRequests.Zipcode,
                        Strmonth = businessRequests.Birthdate.Value.ToString("MMMM"),
                        Intyear = businessRequests.Birthdate.Value.Year,
                        Intdate = businessRequests.Birthdate.Value.Day
                    };
                    _context.Requestclients.Add(patientInfo);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Request Submitted Successfully";
                    return RedirectToAction("Index", "PatientLogin");
                }


                // If user is not exists then new User Creation
                // Create Request Without UserId -> When Account is Created then Assign USERID to Request Table
                var newRequest = new Request
                    {
                        Symptoms = businessRequests.Symptoms,
                        Roomnoofpatient = businessRequests.Roomnoofpatient,
                        Status = 1, // For Unassigned,
                        Firstname = businessRequests.BusinessFirstname,
                        Lastname = businessRequests.BusinessLastname,
                        Phonenumber = businessRequests.BusinessPhonenumber,
                        Email = businessRequests.BusinessEmail,
                        Relationname = "Business",
                        PropertyName = businessRequests.PropertyName,
                        Requesttypeid = 2 // For Family/Friends
                    };
                    _context.Requests.Add(newRequest);
                    await _context.SaveChangesAsync();

                var newPatientInfo = new Requestclient
                {
                        Requestid = newRequest.Id,
                        Firstname = businessRequests.Firstname,
                        Lastname = businessRequests.Lastname,
                        Phonenumber = businessRequests.Mobile,
                        Email = businessRequests.Email,
                        Street = businessRequests.Street,
                        City = businessRequests.City,
                        State = businessRequests.State,
                        Zipcode = businessRequests.Zipcode,
                        Strmonth = businessRequests.Birthdate.Value.ToString("MMMM"),
                        Intyear = businessRequests.Birthdate.Value.Year,
                        Intdate = businessRequests.Birthdate.Value.Day
                };
                _context.Requestclients.Add(newPatientInfo);
                await _context.SaveChangesAsync();
                
                TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                return RedirectToAction("Index", "PatientLogin");
            }
            catch{
                // Log the exception or handle it accordingly
                ModelState.AddModelError("", "An error occurred while saving the data.");
                TempData["error"] = "An error occurred while saving the data.";
                
                return View(nameof(Business), businessRequests);
            }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
