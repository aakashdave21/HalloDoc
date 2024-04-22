using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using HalloDocMVC.Services;

namespace HalloDocMVC.Controllers;

[Area("Patient")]
public class RequestController : Controller
{
    private readonly IPatientRequestService _patientRequestService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IUtilityService _utilityService;

    public RequestController(IPatientRequestService patientRequestService,IWebHostEnvironment hostingEnvironment,IUtilityService utilityService)
    {
        _patientRequestService = patientRequestService;
        _hostingEnvironment = hostingEnvironment;
        _utilityService = utilityService;
    }


    // Patient Request Page
    public IActionResult Patient()
    {
        PatientRequestViewModel newPatientRequest = new();
        newPatientRequest.AllRegionList = _patientRequestService.GetAllRegions();
        return View(newPatientRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatientPost(PatientRequestViewModel viewRequest,IFormFile file)
    {   
            if (!ModelState.IsValid)
            {   
                viewRequest.AllRegionList = _patientRequestService.GetAllRegions();
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Patient), viewRequest); // Return the view with validation errors
            }
       
        try
        {
            // File Upload Logic
            if(file!=null && file.Length > 0){
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                // Ensure the uploads folder exists, create it if not
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                // Generate a unique file name for the uploaded file
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                // Combine the uploads folder path with the unique file name to get the full file path
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Adding or copying the uploaded file to Server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Update the view model with the file path
                viewRequest.FilePath = filePath;
            }

                string hashedPassword = PasswordHasher.HashPassword(viewRequest.Passwordhash);
                viewRequest.Passwordhash = hashedPassword;

                _patientRequestService.ProcessPatientRequestAsync(viewRequest);
                TempData["success"] = "Request Submitted Successfully";
                return RedirectToAction("Index", "PatientLogin");

        }
        catch (Exception)
        {
            ModelState.AddModelError("", "An error occurred while saving the data.");
            TempData["error"] = "An error occurred while saving the data."; 
            return View(nameof(Patient), viewRequest);
        }
    }


    [HttpPost]
    public IActionResult VerifyEmail(string email)
    {
        try
        {
            var userEmail = _patientRequestService.GetUserByEmail(email);
            // var userEmail = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
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
        catch (Exception)
        {
            TempData["error"] = "An error occurred while Verifying the Email.";
            return View(nameof(Patient));
        }
    }


    // Family Request Page
    public IActionResult Family()
    {
        FamilyRequestViewModel newFamilyRequest = new();
        newFamilyRequest.AllRegionList = _patientRequestService.GetAllRegions();
        return View(newFamilyRequest);
    }

    public async Task<IActionResult> FamilyPost(FamilyRequestViewModel familyRequest,IFormFile file){
            if (!ModelState.IsValid)
            {   
                familyRequest.AllRegionList = _patientRequestService.GetAllRegions();
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Family), familyRequest); // Return the view with validation errors
            }

            try{
                // File Upload Logic
                if(file!=null && file.Length > 0){
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    familyRequest.FilePath = filePath;
                }
                int userId = _patientRequestService.ProcessFamilyRequestAsync(familyRequest);

                if(userId!=0){
                    TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                    SendCreationLink(userId);
                }else{
                    TempData["success"] = "Request Submitted Successfully";
                }
                
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
        ConciergeRequestViewModel newConciergeRequest = new();
        newConciergeRequest.AllRegionList = _patientRequestService.GetAllRegions();
        return View(newConciergeRequest);
    }

    public async Task<IActionResult> ConciergePost(ConciergeRequestViewModel conciergeRequest){
            if (!ModelState.IsValid)
            {   
                conciergeRequest.AllRegionList = _patientRequestService.GetAllRegions();
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Concierge), conciergeRequest); // Return the view with validation errors
            }
            try{
                int userId = _patientRequestService.ProcessConciergeRequestAsync(conciergeRequest);
                
                if(userId!=0){
                    TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                    SendCreationLink(userId);
                }else{
                    TempData["success"] = "Request Submitted Successfully";
                }

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
         BusinessRequestViewModel newBusinessRequest = new();
        newBusinessRequest.AllRegionList = _patientRequestService.GetAllRegions();
        return View(newBusinessRequest);
    }

        public async Task<IActionResult> BusinessPost(BusinessRequestViewModel businessRequests){
            if (!ModelState.IsValid)
            {   
                businessRequests.AllRegionList = _patientRequestService.GetAllRegions();
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(Business), businessRequests); // Return the view with validation errors
            }

            try{
                int userId = _patientRequestService.ProcessBusinessRequestAsync(businessRequests);
                 if(userId!=0){
                    TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                    SendCreationLink(userId);
                }else{
                    TempData["success"] = "Request Submitted Successfully";
                }
                return RedirectToAction("Index", "PatientLogin");
            }
            catch{
                // Log the exception or handle it accordingly
                ModelState.AddModelError("", "An error occurred while saving the data.");
                TempData["error"] = "An error occurred while saving the data.";
                
                return View(nameof(Business), businessRequests);
            }
    }

    private void SendCreationLink(int userId){
        try
        {
            // Activation Email Sent To Patient
                string token = Guid.NewGuid().ToString();
                var createAccountLink = Url.Action("Index","SignUp",new { area = "Patient", userId, token }, Request.Scheme);
                DateTime expirationTime = DateTime.UtcNow.AddHours(1);
                _patientRequestService.StoreActivationToken(userId,token,expirationTime);
                string rcvrMail = "aakashdave21@gmail.com";
                string message = $"Please click the following link to reset your password: <a href='{createAccountLink}'>{createAccountLink}</a>";
                _utilityService.EmailSend(rcvrMail,message,"Account Creation Link",null,3,null,null);
        }
        catch (Exception)
        {
            TempData["error"] = "Error While Sending Mail";
            throw;
        }
                
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
