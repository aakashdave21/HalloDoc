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
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices.JavaScript;
using System.Globalization;

namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IDashboardService _dashboardService;
    private readonly IUtilityService _utilityService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public DashboardController(ILogger<DashboardController> logger, IAdminDashboardService adminDashboardService, IDashboardService dashboardService, IWebHostEnvironment hostingEnvironment,IUtilityService utilityService)
    {
        _logger = logger;
        _adminDashboardService = adminDashboardService;
        _dashboardService = dashboardService;
        _hostingEnvironment = hostingEnvironment;
        _utilityService = utilityService;
    }


    public async Task<IActionResult> Index(string status)
    {
        (List<RequestViewModel> req, int totalCount) myresult;
        ViewBag.statusType = string.IsNullOrEmpty(status) ? "new" : status;

        var viewModel = new AdminDashboardViewModel();
        var countDictionary = _adminDashboardService.CountRequestByType();

        viewModel.RegionList = await _adminDashboardService.GetRegions();

        viewModel.NewState = countDictionary["new"];
        viewModel.PendingState = countDictionary["pending"];
        viewModel.ActiveState = countDictionary["active"];
        viewModel.ConcludeState = countDictionary["conclude"];
        viewModel.ToCloseState = countDictionary["close"];
        viewModel.UnPaidState = countDictionary["unpaid"];

        string? searchBy = Request.Query["searchBy"];
        int pageNumber = Request.Query.TryGetValue("pageNumber", out var pageNumberValue) ? int.Parse(pageNumberValue) : 1;
        
        int pageSize = Request.Query.TryGetValue("pageSize", out var pageSizeValue) ? int.Parse(pageSizeValue) : 5;

        int reqType = 0;
        ViewBag.currentPage = pageNumber;
        ViewBag.currentPageSize = pageSize;

        int startIndex = (pageNumber - 1) * pageSize + 1;

        if (!string.IsNullOrEmpty(Request.Query["requesttype"]))
        {
            int.TryParse(Request.Query["requesttype"], out reqType);
        }
        switch (status)
        {
            case "new":
                myresult = _adminDashboardService.GetNewStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "pending":
                myresult = _adminDashboardService.GetPendingStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "active":
                myresult = _adminDashboardService.GetActiveStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "conclude":
                myresult = _adminDashboardService.GetConcludeStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "close":
                myresult = _adminDashboardService.GetCloseStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            case "unpaid":
                myresult = _adminDashboardService.GetUnpaidStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
            default:
                myresult = _adminDashboardService.GetNewStatusRequest(searchBy, reqType, pageNumber, pageSize);
                viewModel.TotalPage = myresult.totalCount;
                viewModel.Requests = myresult.req;
                viewModel.PageRangeEnd = Math.Min(startIndex + pageSize - 1, myresult.totalCount);
                viewModel.NoOfPage = (int)Math.Ceiling((double)myresult.totalCount / pageSize);
                viewModel.PageRangeStart = myresult.totalCount == 0 ? 0 : startIndex;
                break;
        }

        // Check if the request is made via AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // var JsonData = new {
            //     PartialView = PartialView("_AdminDashboardTable", viewModel).RenderToString(),
            //     otherData = viewModel
            // };

            // return Json(JsonData);
            
            return PartialView("_AdminDashboardTable", viewModel);
        }
        else
        {
            return View(viewModel);
        }
    }

    public IActionResult CountCards(){
        try
        {
            var countDictionary = _adminDashboardService.CountRequestByType();
            return Json(countDictionary);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
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
            ViewNotesViewModel viewnotes = _adminDashboardService.GetViewNotesDetails(id);
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
            _adminDashboardService.SaveAdditionalNotes(viewnotes?.AdditionalNote, viewnotes.NoteId, viewnotes.ReqId);
            TempData["success"] = "Updated Successfully";
            return Redirect("/admin/dashboard/ViewNotes/" + viewnotes.ReqId);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    public async Task<IActionResult> GetCaseTag()
    {
        try
        {
            var casetags = await _adminDashboardService.GetCaseTag();
            return Ok(casetags);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> CancleCase(IFormCollection formData)
    {
        try
        {
            var reqId = formData["reqId"];
            var reason = formData["reason"];
            var additionalNotes = formData["additionalNotes"];
            _adminDashboardService.CancleRequestCase(int.Parse(reqId), reason, additionalNotes);
            TempData["success"] = "Request Cancelled Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
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
    [HttpPost]
    public async Task<IActionResult> AssignCase(IFormCollection formData)
    {
        try
        {
            string? Description = formData["description"];
            string? PhysicianId = formData["physician"];
            string? ReqId = formData["reqId"];
            int? AdminId = null;

            await _adminDashboardService.AssignRequestCase(int.Parse(ReqId), int.Parse(PhysicianId), AdminId, Description);

            TempData["success"] = "Request Assigned Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> BlockCase(IFormCollection formData)
    {
        try
        {
            string? Reason = formData["reason"];
            string? ReqId = formData["reqId"];
            int? AdminId = null;

            await _adminDashboardService.BlockRequestCase(int.Parse(ReqId), AdminId, Reason);

            TempData["success"] = "Request Blocked Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> ClearCase(int RequestId)
    {
        try
        {
            _adminDashboardService.SetClearCase(RequestId);
            TempData["success"] = "Request Cleared Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> TransferCase(IFormCollection formData){
         try
        {   
            int reqId = int.Parse(formData["reqId"]);
            int oldphyId = int.Parse(formData["phyId"]);
            int physician = int.Parse(formData["physician"]);
            string description = formData["description"];
            _adminDashboardService.SetTransferCase(reqId,oldphyId,physician,description);
            TempData["success"] = "Request Transfered Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
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
                UploaderName = d.Request.Createduser != null ? d.Request.Createduser.Firstname : d.Request.Firstname,
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
            // string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            // if (!System.IO.File.Exists(filePath))
            // {
            //     TempData["error"] = "File not found";
            //     return RedirectToAction("ViewUploads", new { RequestId = reqId });
            // }

            // // Delete the file
            // System.IO.File.Delete(filePath);

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

            //    foreach (var filename in fileNames)
            //    {
            //         string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);
            //         if (!System.IO.File.Exists(filePath))
            //         {
            //             TempData["error"] = "File not found";
            //             return RedirectToAction("ViewUploads", new { RequestId = reqId });
            //         }
            //         // Delete the file
            //         System.IO.File.Delete(filePath);
            //    }

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
    public async Task<IActionResult> UploadFile(IFormFile file, int requestId)
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

            // We have to add Admin Id here
            _dashboardService.UploadFileFromDocument(fileName, requestId, AdminId);
            TempData["success"] = "Uploaded Successfully";

            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
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

    public IActionResult SendOrder(int RequestId){
        
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
    public IActionResult SendOrder(SendOrderViewModel sendOrders){
        if(sendOrders.ProfessionId == 0){
            ModelState.AddModelError("ProfessionId","Please Select Profession");
        }
        if(sendOrders.BusinessId == 0){
            if(sendOrders.ProfessionId != 0){
                ModelState.AddModelError("BusinessId","Please Choose Profession Again, Then Select Business");
            }else{
                ModelState.AddModelError("BusinessId","Please Select Business");
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
            return RedirectToAction("SendOrder",new {RequestId = sendOrders.ReqId});
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("SendOrder",new {RequestId = sendOrders.ReqId});
        }
    }
    public IActionResult GetBusinessByProfession(int professionId){
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
    public IActionResult GetBusinessDetails(int businessId){
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
    public IActionResult SendAgreement(IFormCollection formData){
        try
        {
            int reqId = int.Parse(formData["request_id"]);
            string Email = formData["Email"];
            string Mobile = formData["Mobile"];
            
            string token = Guid.NewGuid().ToString();
            string callbackUrl = Url.Action("Index", "Agreement", new { area="Patient", reqId, token }, protocol: HttpContext.Request.Scheme);
            DateTime expirationTime = DateTime.UtcNow.AddHours(1);

            _adminDashboardService.StoreAcceptToken(reqId,token,expirationTime);

            Console.WriteLine(callbackUrl);

            string rcvrMail = "aakashdave21@gmail.com";
             Services.SmsSender.SendSMS();
            //  _utilityService.EmailSend(callbackUrl,rcvrMail);
            TempData["success"] = "Agreement Sent To Patient";
            return Ok();
        }
        catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error occurred while fetching businesses: " + ex.Message);
        }
    }


    // Close Case
    public IActionResult CloseCase(string RequestId){
        try
        {
            CloseCaseViewModel CloseCaseView = _adminDashboardService.CloseCase(int.Parse(RequestId));
            return View(CloseCaseView);
        }catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    [HttpPost]
    public IActionResult CloseCasePost(CloseCaseViewModel closeCaseView){
        try
        {
            _adminDashboardService.CloseCaseSubmit(closeCaseView.ReqId);
            TempData["success"] = "Case Closed Successfully";

            return RedirectToAction("Index");
        }catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CloseCaseEdit(IFormCollection formdata){
        try
        {   
            var Email = formdata["Email"];
            var Phone = formdata["phone"];
            var patientId = int.Parse(formdata["patientId"]);
            var requestId = int.Parse(formdata["ReqId"]);
            _adminDashboardService.EditPatientInfo(Email,Phone,patientId,requestId);
            TempData["success"] = "Edited Successfully!";
            return Json(new { success = true, message = "Edited successfully" });

        }catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
        }
    }

    public IActionResult Encounter(){
        return View();
    }

    [HttpPost]
    public IActionResult ConsultEncounter(string requestId){
        try
        {   
            Console.WriteLine(requestId);
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            return Ok(new {message=requestId});

        }catch (System.Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
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
