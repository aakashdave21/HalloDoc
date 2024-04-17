using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;
using HalloDocService.ViewModels;
using HalloDocService.Admin.Interfaces;
using HalloDocRepository.DataModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Interfaces;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using HalloDocService.Provider.Interfaces;

namespace HalloDocMVC.Controllers.Provider;

[Area("Provider")]
[Authorize(Roles = "Provider")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IDashboardService _dashboardService;
    private readonly IUtilityService _utilityService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IProviderDashboardService _providerService;

    private readonly IPatientRequestService _patientRequestService;

    public DashboardController(ILogger<DashboardController> logger, IAdminDashboardService adminDashboardService, IDashboardService dashboardService, IWebHostEnvironment hostingEnvironment, IUtilityService utilityService, IPatientRequestService patientRequestService, IProviderDashboardService providerService)
    {
        _logger = logger;
        _adminDashboardService = adminDashboardService;
        _dashboardService = dashboardService;
        _hostingEnvironment = hostingEnvironment;
        _utilityService = utilityService;
        _patientRequestService = patientRequestService;
        _providerService = providerService;
    }


    public async Task<IActionResult> Index(string status)
    {
        int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
        string? searchBy = null;
        if (Request.Query["searchBy"] != "null") searchBy = Request.Query["searchBy"];
        int pageNumber = Request.Query.TryGetValue("pageNumber", out var pageNumberValue) ? int.Parse(pageNumberValue) : 1;
        int pageSize = Request.Query.TryGetValue("pageSize", out var pageSizeValue) ? int.Parse(pageSizeValue) : 5;
        int reqType = 0;
        if (!string.IsNullOrEmpty(Request.Query["requesttype"]))
        {
            int.TryParse(Request.Query["requesttype"], out reqType);
        }
        AdminDashboardViewModel viewModel = new();
        var result = _providerService.GetDashboardRequests(status, searchBy, reqType, pageNumber, pageSize, AspUserId);
        var (requests, totalCount) = result;
        ViewBag.statusType = string.IsNullOrEmpty(status) ? "new" : status;

        var countDictionary = _providerService.CountRequestByType(AspUserId);
        viewModel.NewState = countDictionary["new"];
        viewModel.PendingState = countDictionary["pending"];
        viewModel.ActiveState = countDictionary["active"];
        viewModel.ConcludeState = countDictionary["conclude"];
        ViewBag.currentPage = pageNumber;
        ViewBag.currentPageSize = pageSize;
        int startIndex = (pageNumber - 1) * pageSize + 1;
        viewModel.TotalPage = totalCount;
        viewModel.Requests = requests;
        viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, totalCount);
        viewModel.NoOfPage = (int)Math.Ceiling((double)totalCount / pageSize);
        viewModel.PageRangeStart = totalCount == 0 ? 0 : startIndex;

        // Check if the request is made via AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_ProviderDashboardTable", viewModel);
        }
        else
        {
            return View(viewModel);
        }
    }

    public IActionResult CountCards()
    {
        try
        {
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            var countDictionary = _providerService.CountRequestByType(AspUserId);
            return Json(countDictionary);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public IActionResult AcceptRequest(int reqId)
    {
        try
        {
            _providerService.AcceptRequest(reqId);
            TempData["success"] = "Request Accepted";
            return RedirectToAction("Index");
        }
        catch (System.Exception e)
        {
            return BadRequest(new { message = "Error Occured" + e });
        }
    }


    public IActionResult ViewCase(int id)
    {
        try
        {
            ViewCaseViewModel viewcase = _adminDashboardService.GetViewCaseDetails(id);
            Console.WriteLine(viewcase.Phone);
            return View("ViewCase", viewcase);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public IActionResult ViewNotes(int id)
    {
        try
        {
            ViewNotesViewModel viewnotes = _adminDashboardService.GetViewNotesDetails(id, 2);
            if (viewnotes != null)
            {
                return View("ViewNotes", viewnotes);
            }
            else
            {
                return View("ViViewNotesew");
            }
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");

        }
    }

    [HttpPost]
    public IActionResult SaveViewNotes(ViewNotesViewModel viewnotes)
    {
        try
        {
            _adminDashboardService.SaveAdditionalNotes(viewnotes?.AdditionalNote, viewnotes.NoteId, viewnotes.ReqId, 2);
            TempData["success"] = "Updated Successfully";
            return Redirect("/provider/dashboard/ViewNotes/" + viewnotes.ReqId);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/provider/Dashboard/Index");
        }
    }

    public async Task<IActionResult> GetRegions()
    {
        try
        {
            var regions = await _adminDashboardService.GetRegions();
            return Ok(regions);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public async Task<IActionResult> GetPhysicians(int RegionId)
    {
        try
        {
            var physicians = await _adminDashboardService.GetPhysicianByRegion(RegionId);
            if (physicians == null)
            {
                return Ok(new List<Physician>());
            }
            return Ok(physicians);
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

   
    public async Task<IActionResult> ViewUploads(int RequestId)
    {
        try
        {

            var DocumentRecords = _dashboardService.GetAllRequestedDocuments(RequestId);
            var patientData = _adminDashboardService.GetSingleRequest(RequestId);
            ViewBag.PatientName = patientData.User != null ? (patientData.User.Firstname + " " + patientData.User.Lastname) :
                 (patientData.Requestclients.FirstOrDefault()?.Firstname.ToUpper() + " " + patientData.Requestclients.FirstOrDefault()?.Lastname.ToUpper());
            ViewBag.userId = User.FindFirstValue("UserId");
            ViewBag.requestId = RequestId;

            List<ViewDocuments> viewModel = DocumentRecords.Select(d => new ViewDocuments
            {
                DocumentId = d.Id,
                FilePath = d.Filename,
                FileName = Path.GetFileName(d.Filename),
                UploaderName = d.Request.Createduser != null ? d.Request.Createduser.Email : d.Request.Firstname,
                UploadDate = d.Createddate.ToString("yyyy-MM-dd"),
                PatientName = d.Request.User != null ? (d.Request.User.Firstname + " " + d.Request.User.Lastname) :
                 (d.Request.Requestclients.FirstOrDefault()?.Firstname.ToUpper() + " " + d.Request.Requestclients.FirstOrDefault()?.Lastname.ToUpper())
            }).ToList();

            return View(viewModel);

        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return View("Index");
        }
    }
    public async Task<IActionResult> SingleDownload(string fileName, int reqId)
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
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }

    public async Task<IActionResult> SelectedDownload(string[] fileNames, int reqId)
    {

        if (fileNames.Length == 0 || fileNames == null)
        {
            TempData["error"] = "Please Choose File ! ";
            return Json(new { redirectTo = Url.Action("ViewUploads", "Dashboard", new { area = "Admin", requestId = reqId }) });
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
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }

    public async Task<IActionResult> SingleDelete(string fileName, int reqId, string docId)
    {
        try
        {

            _adminDashboardService.DeleteDocument(int.Parse(docId));

            TempData["success"] = "File deleted successfully";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }

    public async Task<IActionResult> SelectedDelete(string[] fileNames, int reqId, string[] fileIds)
    {

        if (fileNames.Length == 0 || fileNames == null)
        {
            TempData["error"] = "Please Choose File ! ";
            return Json(new { redirectTo = Url.Action("ViewUploads", "Dashboard", new { area = "Admin", requestId = reqId }) });
        }
        try
        {

            foreach (var fileId in fileIds)
            {
                _adminDashboardService.DeleteDocument(int.Parse(fileId));
            }

            TempData["error"] = "Files Are Deleted !";
            return Json(new { success = true, message = "Deleted Success" });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }
    public async Task<IActionResult> UploadFile(IFormFile file, int requestId, string? Type = null)
    {

        if (file == null)
        {
            TempData["error"] = "Please Choose File !";
            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
        try
        {
            int? AdminId = null;
            string fileName = null;
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                fileName = filePath;
            }

            // We have to add Admin Id here
            _dashboardService.UploadFileFromDocument(fileName, requestId, AdminId);
            TempData["success"] = "Uploaded Successfully";
            if(Type == "conclude"){
                return RedirectToAction("ConcludeCare", new { RequestId = requestId });    
            }
            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            if(Type == "conclude"){
                return RedirectToAction("ConcludeCare", new { RequestId = requestId });    
            }
            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
    }


    public async Task<IActionResult> SendEmailToPatient(string[] fileNames, int reqId, string[] fileIds)
    {
        if (fileNames.Length == 0 || fileNames == null)
        {
            TempData["error"] = "Please Choose File ! ";
            return Json(new { redirectTo = Url.Action("ViewUploads", "Dashboard", new { area = "Admin", requestId = reqId }) });
        }
        try
        {

            string senderEmail = "tatva.dotnet.aakashdave@outlook.com";
            string senderPassword = "Aakash21##";

            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Set up your Account",
                IsBodyHtml = true,
                Body = $"Dear patient, please find attached the files you requested"
            };

            foreach (var fileName in fileNames)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    // Add the file as an attachment to the email
                    mailMessage.Attachments.Add(new Attachment(filePath));
                }
                else
                {
                    // Handle the case where the file does not exist
                    Console.WriteLine($"File '{fileName}' does not exist at path: {filePath}");
                }
            }

            mailMessage.To.Add("aakashdave21@gmail.com");

            client.Send(mailMessage);


            TempData["success"] = "Files Are Send !";
            return Json(new { success = true, message = "File Send Success" });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }

    public IActionResult SendOrder(int RequestId)
    {

        try
        {
            SendOrderViewModel sendOrders = new SendOrderViewModel
            {
                ProfessionLists = _adminDashboardService.GetAllProfessions()
            };
            ViewBag.RequestId = RequestId;
            return View(sendOrders);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("SendOrder", new { RequestId });
        }
    }

    [HttpPost]
    public IActionResult SendOrder(SendOrderViewModel sendOrders)
    {
        if (sendOrders.ProfessionId == 0)
        {
            ModelState.AddModelError("ProfessionId", "Please Select Profession");
        }
        if (sendOrders.BusinessId == 0)
        {
            if (sendOrders.ProfessionId != 0)
            {
                ModelState.AddModelError("BusinessId", "Please Choose Profession Again, Then Select Business");
            }
            else
            {
                ModelState.AddModelError("BusinessId", "Please Select Business");
            }

        }
        if (!ModelState.IsValid)
        {
            sendOrders.ProfessionId = 0;
            sendOrders.ProfessionLists = _adminDashboardService.GetAllProfessions();
            ViewBag.RequestId = sendOrders.ReqId;
            return View(sendOrders);
        }
        try
        {
            _adminDashboardService.AddOrderDetails(sendOrders);
            TempData["success"] = "Order Sent Successfully!";
            return RedirectToAction("SendOrder", new { RequestId = sendOrders.ReqId });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("SendOrder", new { RequestId = sendOrders.ReqId });
        }
    }
    public IActionResult GetBusinessByProfession(int professionId)
    {
        try
        {
            IEnumerable<BusinessList> businessLists = _adminDashboardService.GetBusinessByProfession(professionId);
            return Json(businessLists);
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error occurred while fetching businesses: " + ex.Message);
        }
    }
    public IActionResult GetBusinessDetails(int businessId)
    {
        try
        {
            SendOrderViewModel viewData = _adminDashboardService.GetBusinessDetails(businessId);
            return Json(viewData);
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error occurred while fetching businesses: " + ex.Message);
        }
    }

    // Send Agreements
    public IActionResult SendAgreement(IFormCollection formData)
    {
        try
        {
            int reqId = int.Parse(formData["request_id"]);
            string Email = formData["Email"];
            string Mobile = formData["Mobile"];

            string token = Guid.NewGuid().ToString();
            string callbackUrl = Url.Action("Index", "Agreement", new { area = "Patient", reqId, token }, protocol: HttpContext.Request.Scheme);
            DateTime expirationTime = DateTime.UtcNow.AddHours(1);

            _adminDashboardService.StoreAcceptToken(reqId, token, expirationTime);

            Console.WriteLine(callbackUrl);

            string rcvrMail = "aakashdave21@gmail.com";
            Services.SmsSender.SendSMS();
            TempData["success"] = "Agreement Sent To Patient";
            return Ok();
        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error occurred while fetching businesses: " + ex.Message);
        }
    }




    public IActionResult CreateRequest()
    {
        try
        {
            PatientRequestViewModel newPatientRequest = new();
            newPatientRequest.AllRegionList = _patientRequestService.GetAllRegions();
            return View(newPatientRequest);
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest(PatientRequestViewModel newPatientRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                newPatientRequest.AllRegionList = _patientRequestService.GetAllRegions();
                return View(newPatientRequest);
            }
            newPatientRequest.CreatedById = int.Parse(User.FindFirstValue("AspUserId"));
            _adminDashboardService.CreateRequest(newPatientRequest);
            TempData["Success"] = "Request Created Successfully";
            return RedirectToAction("CreateRequest");
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    

    public IActionResult GetRequestStatusEncounter(string requestId)
    {
        try
        {
            var requestData = _adminDashboardService.GetSingleRequest(int.Parse(requestId));
            return Json(new { success = true, message = "SuccessFully Consulted", requestStatus = requestData.Status });

        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
        }
    }

    public IActionResult Encounter(string RequestId)
    {
        EncounterFormViewModel encounterDetails = _adminDashboardService.GetEncounterDetails(int.Parse(RequestId));
        return View(encounterDetails);
    }

    public IActionResult EncounterPost(EncounterFormViewModel encounterForm)
    {
        try
        {

            _adminDashboardService.SubmitEncounter(encounterForm);
            TempData["success"] = "Report Submitted Successfully";
            return RedirectToAction("Encounter", new { RequestId = encounterForm.ReqId });

        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Encounter", new { RequestId = encounterForm.ReqId });
        }
    }

    [HttpPost]
    public IActionResult ConsultEncounter(string requestId)
    {
        try
        {
            _adminDashboardService.ConsultEncounter(int.Parse(requestId));
            return Json(new { success = true, message = "SuccessFully Consulted", requestId });

        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
        }
    }

    [HttpPost]
    public IActionResult HouseCallEncounter(string requestId, string status)
    {
        try
        {
            _adminDashboardService.HouseCallEncounter(int.Parse(requestId), status);

            return Json(new { success = true, message = "Successfully Completed", requestId, status });

        }
        catch (System.Exception ex)
        {
            return BadRequest("Error: " + ex.Message);
        }
    }
    [HttpPost]
    public IActionResult SendCreationLink(IFormCollection formData)
    {
        try
        {
            string CreateServiceLink = Url.Action("Patient", "Request", new { area = "Patient" }, Request.Scheme);
            string rcvrMail = "aakashdave21@gmail.com";
            _utilityService.EmailSend(CreateServiceLink, rcvrMail);
            TempData["success"] = "Account Creation Link Send !";
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult ConcludeCare(int RequestId)
    {
        try
        {
            CloseCaseViewModel CloseCaseView = _adminDashboardService.CloseCase(RequestId);
            ViewBag.userId = User.FindFirstValue("UserId");
            ViewBag.requestId = RequestId;
            return View(CloseCaseView);
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult ConcludeCarePost(int RequestId, string ProviderNote)
    {
        try
        {
            bool isRequestFinalized = _providerService.CheckEncounterFinalized(RequestId);
            if(isRequestFinalized==false){
                TempData["error"] = "Please Finalize Medical Form Before Conclude Care!";
                return RedirectToAction("ConcludeCare", new {RequestId});
            }
            int? PhyId = int.Parse(User.FindFirstValue("UserId"));
            _providerService.ConcludeCare(RequestId,ProviderNote,PhyId);
            TempData["success"] = "Request Concluded";
            return RedirectToAction("ConcludeCare", new {RequestId});
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Finalized(int Id,int ReqId){
        try
        {
            _providerService.FinalizeForm(Id,ReqId);
            TempData["success"] = "Form Finalized Successfully";
             return RedirectToAction("Index"); 
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
