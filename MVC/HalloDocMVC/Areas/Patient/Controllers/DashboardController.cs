using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using HalloDocRepository.DataModels;
using Microsoft.CodeAnalysis;
using System.IO.Compression;

namespace HalloDocMVC.Controllers;

[Area("Patient")]
[Authorize(Roles = "Patient")]
public class DashboardController : Controller
{

    private readonly IDashboardService _dashboardService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IPatientRequestService _patientRequestService;
    private readonly IUtilityService _utilityService;


    public DashboardController(IDashboardService dashboardService, IWebHostEnvironment hostingEnvironment,IPatientRequestService patientRequestService,IUtilityService utilityService)
    {
        _dashboardService = dashboardService;
        _hostingEnvironment = hostingEnvironment;
        _patientRequestService = patientRequestService;
        _utilityService = utilityService;

    }

    public IActionResult Index()
    {
        var userClaims = User.Claims;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdClaim = User.FindFirstValue("UserId");

        try
        {
            IEnumerable<Request> userRequests = _dashboardService.GetUserRequest(int.Parse(userIdClaim));
            // int[] reqDocId = _dashboardService.GetAllIdOfRequestWiseFile();

            DashboardViewModel viewModel = new DashboardViewModel
            {
                UserRequests = userRequests,
                GetUserRequestType = _dashboardService.GetUserRequestType
            };

            return View(viewModel);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return View();
        }
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    Response.Headers["Pragma"] = "no-cache";
    Response.Headers["Expires"] = "0";
        return RedirectToAction("Index", "PatientLogin");
    }

    public IActionResult UserProfile()
    {
        try
        {
            var userIdClaim = User.FindFirstValue("UserId");
            var userData = _dashboardService.GetUserData(int.Parse(userIdClaim));

            DateTime dates = DateTime.Parse(userData.Birthdate.ToString());
            var bdate = dates.ToString("yyyy-MM-dd");

            UserProfileViewModel userProfileView = new UserProfileViewModel
            {
                Firstname = userData.Firstname,
                Lastname = userData.Lastname,
                Email = userData.Email,
                Mobile = userData.Mobile,
                Street = userData.Street,
                State = userData.State,
                Zipcode = userData.Zipcode,
                City = userData.City,
                Birthdate = bdate,
            };

            return View(userProfileView);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return View();
        }
    }

    [HttpPost]
    public IActionResult Edit(UserProfileViewModel userData)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(UserProfile), userData); // Return the view with validation errors
        }
        try
        {
            var userId = User.FindFirstValue("UserId");
            int id = int.Parse(userId);
            
            _dashboardService.EditUserProfile(id, userData);
            TempData["success"] = "Profile Updated Successfully";
            return RedirectToAction("UserProfile");
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return View("UserProfile");
        }

    }

    public IActionResult Documents(int RequestId)
    {
        try
        {
            var DocumentRecords = _dashboardService.GetAllRequestedDocuments(RequestId);
            ViewBag.userId = User.FindFirstValue("UserId");
            ViewBag.requestId = RequestId;

            var viewModel = DocumentRecords.Select(d => new ViewDocuments
            {
                DocumentId = d.Id,
                FilePath = d.Filename,
                FileName = Path.GetFileName(d.Filename),
                UploaderName = d.Request.Createduser != null ? d.Request.Createduser.Email : d.Request.Firstname,
                UploadDate = d.Createddate.ToString("yyyy-MM-dd"),
                PatientName = d.Request.User.Firstname + " " + d.Request.User.Lastname
            }).ToList();

            return View(viewModel);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return View("Dashboard");
        }
    }

    public async Task<IActionResult> UploadFile(IFormFile file, int requestId)
    {
        if (file == null)
        {
            TempData["error"] = "Please Choose File !";
            return RedirectToAction("Documents", new { requestId });
        }
        try
        {
            string fileName = null;
            // File Upload Logic
            if (file != null && file.Length > 0)
            {
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
                fileName = filePath;
            }

            _dashboardService.UploadFileFromDocument(fileName, requestId, null);
            TempData["success"] = "Uploaded Successfully";

            return RedirectToAction("Documents", new { requestId });
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Documents", new { requestId });
        }
    }

    
    public IActionResult SingleDownload(string fileName,int reqId)
    {
        try
        {
            // Construct the full file path by combining the wwwroot path and the fileName
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                TempData["error"] = "File not found";
                return View("Index");
            }
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", fileName);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return View("Dashboard");
        }
    }

    public IActionResult SelectedDownload(string[] fileNames,int reqId)
    {   

        if(fileNames.Length==0 || fileNames == null){
            TempData["error"] = "Please Choose File ! ";
                return Json(new { redirectTo = Url.Action("Documents", "Dashboard", new { area = "Patient", requestId = reqId }) });

        }

        try
        {       
            var zipName = $"HalloDoc_Documents.zip";
            using (MemoryStream ms = new MemoryStream())
            { 
                using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    //QUery the Products table and get all image content  
                    foreach (var file in fileNames)
                    {

                        var entry = zip.CreateEntry(file);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot\\uploads", file);
                        byte[] buffer = System.IO.File.ReadAllBytes(filePath);
                        using (var fileStream = new MemoryStream(buffer))
                        using (var entryStream = entry.Open())
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                return File(ms.ToArray(), "application/zip", zipName);
            }

        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Documents", new { reqId });
        }
    }

    public IActionResult NewRequest(){
        try
        {
            var userIdClaim = User.FindFirstValue("UserId");
            var userData = _dashboardService.GetUserData(int.Parse(userIdClaim));
            DateTime dates = DateTime.Parse(userData.Birthdate.ToString());
            var bdate = dates.ToString("yyyy-MM-dd");
            PatientRequestViewModel newPatientRequest = new PatientRequestViewModel{
                Firstname = userData.Firstname,
                Birthdate = bdate,
                Lastname = userData.Lastname,
                Email = userData.Email,
                Mobile = userData.Mobile
            };
            newPatientRequest.AllRegionList = _patientRequestService.GetAllRegions();
        
            return View(newPatientRequest);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> NewRequestPost(PatientRequestViewModel viewRequest,IFormFile file){
        Console.WriteLine("ModelState errors:");
        if (!ModelState.IsValid)
        {   
            viewRequest.AllRegionList = _patientRequestService.GetAllRegions();
            TempData["error"] = "Something went wrong! Please enter your details correct";
            return View(nameof(NewRequest), viewRequest); // Return the view with validation errors
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

                _patientRequestService.ProcessPatientRequestAsync(viewRequest);
                TempData["success"] = "Request Submitted Successfully";
                return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while saving the data.");
            TempData["error"] = "An error occurred while saving the data."; 
            return View(nameof(NewRequest), viewRequest);
        }
    }

    public IActionResult AnotherUserRequest(){

        var userIdClaim = User.FindFirstValue("UserId");
        var userData = _dashboardService.GetUserData(int.Parse(userIdClaim));

        FamilyRequestViewModel familyRequest = new FamilyRequestViewModel{
            FamilyFirstname = userData.Firstname,
            FamilyLastname = userData.Lastname,
            FamilyEmail = userData.Email,
            FamilyPhonenumber = userData.Mobile
        };

        familyRequest.AllRegionList = _patientRequestService.GetAllRegions();

        
        return View(familyRequest);
    }

    [HttpPost]
    public async Task<IActionResult> AnotherUserRequestPost(FamilyRequestViewModel familyRequest,IFormFile file){
            if (!ModelState.IsValid)
            {   
                familyRequest.AllRegionList = _patientRequestService.GetAllRegions();
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(nameof(AnotherUserRequest), familyRequest); // Return the view with validation errors
            }

            try{
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
                    familyRequest.FilePath = filePath;
                }
                int userId = _patientRequestService.ProcessFamilyRequestAsync(familyRequest);

                if(userId!=0){
                    TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                    SendCreationLink(userId);
                }else{
                    TempData["success"] = "Request Submitted Successfully";
                }
                _patientRequestService.ProcessFamilyRequestAsync(familyRequest);
                TempData["success"] = "Request Submitted Successfully, Account Activation Link sent to the customer email";
                return RedirectToAction("Index", "PatientLogin");
            }
            catch{
                // Log the exception or handle it accordingly
                ModelState.AddModelError("", "An error occurred while saving the data.");
                TempData["error"] = "An error occurred while saving the data.";
                return View(nameof(AnotherUserRequest), familyRequest);
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
                _utilityService.EmailSend(rcvrMail,message,"Account Creation Link");
        }
        catch (Exception e)
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
