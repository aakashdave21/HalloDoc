using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
// using HalloDocMVC.Data;
// using HalloDocMVC.ViewModels;
using Microsoft.EntityFrameworkCore;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using HalloDocRepository.DataModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using MimeKit;

namespace HalloDocMVC.Controllers;

[Authorize]
public class DashboardController : Controller
{

        private readonly IDashboardService _dashboardService; 
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DashboardController(IDashboardService dashboardService,IWebHostEnvironment hostingEnvironment)
        {
            _dashboardService = dashboardService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(){
            var userClaims = User.Claims;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdClaim = User.FindFirstValue("UserId");
           
            try
            {
                IEnumerable<Request> userRequests = _dashboardService.GetUserRequest(int.Parse(userIdClaim));
                // int[] reqDocId = _dashboardService.GetAllIdOfRequestWiseFile();
                // Console.WriteLine(reqDocId);

                foreach (var item in userRequests)
                {
                    Console.WriteLine(item.NoOfRequests + " <= " + item.Firstname);
                }
                
                DashboardViewModel viewModel = new DashboardViewModel
                { 
                    UserRequests = userRequests,
                    GetUserRequestType = _dashboardService.GetUserRequestType
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                TempData["error"]="Internal Server Error";
                return View();
            }            
        }

        public async Task<IActionResult> LogOut(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","PatientLogin");
        }

        public async Task<IActionResult> UserProfile(){
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                var userData =  _dashboardService.GetUserData(int.Parse(userIdClaim));

                DateTime dates = DateTime.Parse(userData.Birthdate.ToString());
                var bdate = dates.ToString("yyyy-MM-dd");
                
                UserProfileViewModel userProfileView = new UserProfileViewModel{
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
        public async Task<IActionResult> Edit(UserProfileViewModel userData){

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
                    return View(nameof(UserProfile), userData); // Return the view with validation errors
                }
            try
            {
                var userId = User.FindFirstValue("UserId");
                var id = int.Parse(userId);
                _dashboardService.EditUserProfile(id,userData);
                 TempData["success"] = "Profile Updated Successfully";
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                TempData["error"] = "Internal Server Error";
                return View("UserProfile");
            }
            
        }

        [HttpGet("/Dashboard/Documents/{RequestId}")]
        public async Task<IActionResult> Documents(int requestId){
            try
            {
                var DocumentRecords = _dashboardService.GetAllRequestedDocuments(requestId);
                foreach (var item in DocumentRecords)
                {
                    string fileName = Path.GetFileName(item.Filename);
                    Console.WriteLine("File Name :" + fileName);
                    Console.WriteLine("Uploader Name :" + item.Request.Firstname);
                    Console.WriteLine("Uploader Name :" + item.Request.Createduser);
                }

                ViewBag.userId = User.FindFirstValue("UserId");
                ViewBag.requestId = requestId;

                var viewModel = DocumentRecords.Select(d => new ViewDocuments
                {
                    DocumentId = d.Id,
                    FilePath = d.Filename,
                    FileName = Path.GetFileName(d.Filename),
                    UploaderName = d.Request.Createduser != null ? d.Request.Createduser.Firstname : d.Request.Firstname,
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

        public async Task<IActionResult> UploadFile(IFormFile file,int requestId){
            if(file==null){
                 TempData["error"] = "Please Choose File !";
                 return RedirectToAction("Documents", new { requestId });
            }
            try{
                string fileName = null;
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
                    fileName = filePath;
                }

                _dashboardService.UploadFileFromDocument(fileName,requestId);
                TempData["success"] = "Uploaded Successfully";
                
                 return RedirectToAction("Documents", new { requestId });
            }catch(Exception e){
                TempData["error"] = "Internal Server Error";
                 return RedirectToAction("Documents", new { requestId });
            }
        }

        [HttpGet("/Dashboard/SingleDownload/{FileName}")]
        public async Task<IActionResult> SingleDownload(string filePath){
            try
            {
                 // Read file content
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Determine the content type based on the file extension
                string contentType = "application/octet-stream"; // Default content type
                string fileExtension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(fileExtension))
                {
                    contentType = GetContentType(fileExtension);
                }

                // Return the file as a FileContentResult
                return File(fileBytes, contentType, Path.GetFileName(filePath));
                
            }
            catch (Exception)
            {
                TempData["error"] = "Internal Server Error";
                return View("Dashboard");
            }
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string GetContentType(string fileExtension)
    {
        // Map file extensions to content types
        switch (fileExtension.ToLower())
        {
            case ".txt": return "text/plain";
            case ".pdf": return "application/pdf";
            case ".doc": return "application/msword";
            case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            // Add more mappings as needed
            default: return "application/octet-stream";
        }
    }
}

