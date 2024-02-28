using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace HalloDocMVC.Controllers;

public class RequestController : Controller
{
    private readonly IPatientRequestService _patientRequestService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public RequestController(IPatientRequestService patientRequestService,IWebHostEnvironment hostingEnvironment)
    {
        _patientRequestService = patientRequestService;
        _hostingEnvironment = hostingEnvironment;
    }


    // Patient Request Page
    public IActionResult Patient()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatientPost(PatientRequestViewModel viewRequest,IFormFile file)
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

                await _patientRequestService.ProcessPatientRequestAsync(viewRequest);
                TempData["success"] = "Request Submitted Successfully";
                return RedirectToAction("Index", "PatientLogin");

        }
        catch (Exception ex)
        {
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

    public async Task<IActionResult> FamilyPost(FamilyRequestViewModel familyRequest,IFormFile file){

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
                await _patientRequestService.ProcessFamilyRequestAsync(familyRequest);
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
                await _patientRequestService.ProcessConciergeRequestAsync(conciergeRequest);
                
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

                await _patientRequestService.ProcessBusinessRequestAsync(businessRequests);
                
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
