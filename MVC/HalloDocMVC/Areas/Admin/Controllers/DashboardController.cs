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
using ClosedXML.Excel;
using HalloDocRepository.CustomModels;

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
    private readonly IPatientRequestService _patientRequestService;

    public DashboardController(ILogger<DashboardController> logger, IAdminDashboardService adminDashboardService, IDashboardService dashboardService, IWebHostEnvironment hostingEnvironment, IUtilityService utilityService, IPatientRequestService patientRequestService)
    {
        _logger = logger;
        _adminDashboardService = adminDashboardService;
        _dashboardService = dashboardService;
        _hostingEnvironment = hostingEnvironment;
        _utilityService = utilityService;
        _patientRequestService = patientRequestService;
    }

    [HttpGet]
    [Route("/admin")]
    [Route("/admin/dashboard/")]
    [Route("/admin/dashboard/Index")]
    public IActionResult Index()
    {
        DashboardRequestQuery dashboardRequestQuery = new();
        string? status = Request.Query["status"];
        string? StatusVal = string.IsNullOrEmpty(status) ? "new" : status;
        ViewBag.statusType = StatusVal;
        dashboardRequestQuery.Status = StatusVal;
        if (Request.Query["searchBy"] != "null") dashboardRequestQuery.SearchBy = Request.Query["searchBy"];
        if (Request.Query["region"] != "0") dashboardRequestQuery.Region = !string.IsNullOrEmpty(Request.Query["region"]) ? int.TryParse(Request.Query["region"], out int regionValue) ? regionValue : 0 : 0;
        dashboardRequestQuery.PageNumber = Request.Query.TryGetValue("pageNumber", out var pageNumberValue) ? int.Parse(pageNumberValue) : 1;
        dashboardRequestQuery.PageSize = Request.Query.TryGetValue("pageSize", out var pageSizeValue) ? int.Parse(pageSizeValue) : 5;
        ViewBag.currentPage = dashboardRequestQuery.PageNumber;
        ViewBag.currentPageSize = dashboardRequestQuery.PageSize;
        if (int.TryParse(Request.Query["requesttype"], out int reqType)) dashboardRequestQuery.RequestTypeId = reqType;
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView("_AdminDashboardTable", _adminDashboardService.GetDashboardRequests(dashboardRequestQuery));
        else return View(_adminDashboardService.GetDashboardRequests(dashboardRequestQuery));
    }

    public IActionResult CountCards()
    {
        try
        {
            var countDictionary = _adminDashboardService.CountRequestByType();
            return Json(countDictionary);
        }
        catch (Exception e)
        {
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
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error!";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public IActionResult ViewNotes(int id)
    {
        try
        {
            ViewNotesViewModel viewnotes = _adminDashboardService.GetViewNotesDetails(id);
            return View("ViewNotes", viewnotes);
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error!";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    [HttpPost]
    public IActionResult SaveViewNotes(ViewNotesViewModel viewnotes)
    {
        try
        {
            _adminDashboardService.SaveAdditionalNotes(viewnotes?.AdditionalNote, viewnotes.NoteId, viewnotes.ReqId, 1);
            TempData["success"] = "Updated Successfully";
            return Redirect("/admin/dashboard/ViewNotes/" + viewnotes.ReqId);
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error!";
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
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public IActionResult CancleCase(IFormCollection formData)
    {
        try
        {
            string? reqId = formData["reqId"];
            string? reason = formData["reason"];
            string? additionalNotes = formData["additionalNotes"];
            int AdminAspId = int.Parse(User.FindFirstValue("UserId"));
            _adminDashboardService.CancleRequestCase(int.Parse(reqId), reason, additionalNotes,AdminAspId);
            TempData["success"] = "Request Cancelled Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    public IActionResult GetRegions()
    {
        try
        {
            return Ok(_adminDashboardService.GetRegions());
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
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public IActionResult AssignCase(IFormCollection formData)
    {
        try
        {
            string? Description = formData["description"];
            string? PhysicianId = formData["physician"];
            string? ReqId = formData["reqId"];
            int? AdminId = null;

            _adminDashboardService.AssignRequestCase(int.Parse(ReqId), int.Parse(PhysicianId), AdminId, Description);

            TempData["success"] = "Request Assigned Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
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
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    [HttpPost]
    public IActionResult ClearCase(int RequestId)
    {
        try
        {
            _adminDashboardService.SetClearCase(RequestId);
            TempData["success"] = "Request Cleared Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }

    [HttpPost]
    public IActionResult TransferCase(IFormCollection formData)
    {
        try
        {
            int reqId = int.Parse(formData["reqId"]);
            int oldphyId = int.Parse(formData["phyId"]);
            int physician = int.Parse(formData["physician"]);
            string description = formData["description"];
            _adminDashboardService.SetTransferCase(reqId, oldphyId, physician, description);
            TempData["success"] = "Request Transfered Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Admin/Dashboard/Index");
        }
    }
    public IActionResult ViewUploads(int RequestId)
    {
        try
        {
            var DocumentRecords = _dashboardService.GetAllRequestedDocuments(RequestId);
            var patientData = _adminDashboardService.GetSingleRequest(RequestId);
            ViewBag.PatientName = patientData.User != null ? (patientData.User.Firstname + " " + patientData.User.Lastname) :
                 (patientData.Requestclients.FirstOrDefault()?.Firstname.ToUpper() + " " + patientData.Requestclients.FirstOrDefault()?.Lastname?.ToUpper());
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
                 (d.Request.Requestclients.FirstOrDefault()?.Firstname.ToUpper() + " " + d.Request.Requestclients.FirstOrDefault()?.Lastname?.ToUpper())
            }).ToList();

            return View(viewModel);

        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return View("Index");
        }
    }
    public IActionResult SingleDownload(string fileName, int reqId)
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

    public IActionResult SelectedDownload(string[] fileNames, int reqId)
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

    public IActionResult SingleDelete(string fileName, int reqId, string docId)
    {
        try
        {
            _adminDashboardService.DeleteDocument(int.Parse(docId));
            TempData["success"] = "File deleted successfully";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
        catch (RecordNotFoundException)
        {
            TempData["error"] = "Record not found!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("ViewUploads", new { RequestId = reqId });
        }
    }

    public IActionResult SelectedDelete(string[] fileNames, int reqId, string[] fileIds)
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

            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
    }


    public IActionResult SendEmailToPatient(string[] fileNames, int reqId, string[] fileIds)
    {
        if (fileNames.Length == 0 || fileNames == null)
        {
            TempData["error"] = "Please Choose File ! ";
            return Json(new { redirectTo = Url.Action("ViewUploads", "Dashboard", new { area = "Admin", requestId = reqId }) });
        }
        try
        {
            string[] filePaths = new string[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                filePaths[i] = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileNames[i]);
            }
            _utilityService.EmailSend("aakashdave21@gmail.com", "Dear patient, please find attached the files you requested", "HalloDoc Attachments", filePaths, 3, reqId, null, null);
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
            string rcvrMail = "aakashdave21@gmail.com";
            Services.SmsSender.SendSMS();
            //  _utilityService.EmailSend(callbackUrl,rcvrMail);
            TempData["success"] = "Agreement Sent To Patient";
            return Ok();
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error occurred while fetching businesses: " + ex.Message);
        }
    }


    // Close Case
    public IActionResult CloseCase(string RequestId)
    {
        try
        {
            CloseCaseViewModel CloseCaseView = _adminDashboardService.CloseCase(int.Parse(RequestId));
            return View(CloseCaseView);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    [HttpPost]
    public IActionResult CloseCasePost(CloseCaseViewModel closeCaseView)
    {
        try
        {
            _adminDashboardService.CloseCaseSubmit(closeCaseView.ReqId);
            TempData["success"] = "Case Closed Successfully";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CloseCaseEdit(IFormCollection formdata)
    {
        try
        {
            var Email = formdata["Email"];
            var Phone = formdata["phone"];
            var patientId = int.Parse(formdata["patientId"]);
            var requestId = int.Parse(formdata["ReqId"]);
            _adminDashboardService.EditPatientInfo(Email, Phone, patientId, requestId);
            TempData["success"] = "Edited Successfully!";
            return Json(new { success = true, message = "Edited successfully" });

        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
        }
    }

    public IActionResult GetRequestStatusEncounter(string requestId)
    {
        try
        {
            var requestData = _adminDashboardService.GetSingleRequest(int.Parse(requestId));
            return Json(new { success = true, message = "SuccessFully Consulted", requestStatus = requestData.Status });
        }
        catch (Exception ex)
        {
            TempData["error"] = "Internal Server Error";
            return BadRequest("Error: " + ex.Message);
        }
    }

    public IActionResult Encounter(string RequestId)
    {
        var encounterDetails = _adminDashboardService.GetEncounterDetails(int.Parse(RequestId));
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
        catch (Exception)
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
        catch (Exception ex)
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
        catch (Exception ex)
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
            string? rcvrMail = "aakashdave21@gmail.com";
            string message = $"Please click the following link to create a new Request: <a href=\"{CreateServiceLink}\">{CreateServiceLink}</a>";
            _utilityService.EmailSend("aakashdave21@gmail.com", message, "Create Your Request.", null, 3, null, null, null);
            TempData["success"] = "Account Creation Link Send !";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    public IActionResult ExportAll()
    {
        try
        {
            var data = _adminDashboardService.FetchAllRequest();
            if (data == null)
            {
                // Handle case where data is null
                TempData["error"] = "No data found";
                return RedirectToAction("Index");
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            // Set header text, style, and borders
            var headerStyle = worksheet.Range("A1:E1").Style;
            headerStyle.Font.Bold = true;
            headerStyle.Fill.BackgroundColor = XLColor.LightGray;
            headerStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
            headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
            headerStyle.Font.FontColor = XLColor.FromHtml("#DCE6F1");

            worksheet.Cell(1, 1).Value = "PatientName";
            worksheet.Column(1).Width = 15;

            worksheet.Cell(1, 2).Value = "EmailId";
            worksheet.Column(2).Width = 30;

            worksheet.Cell(1, 3).Value = "Dob";
            worksheet.Column(3).Width = 15;

            worksheet.Cell(1, 4).Value = "Patient Contact";
            worksheet.Column(4).Width = 15;

            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Column(5).Width = 50;

            int row = 2;
            foreach (var item in data)
            {
                // Check for null references in the item and related entities
                if (item != null && item.Requestclients != null && item.User != null)
                {
                    worksheet.Cell(row, 1).Value = item.Requestclients.FirstOrDefault()?.Firstname + " " + item.Requestclients.FirstOrDefault()?.Lastname;
                    worksheet.Cell(row, 2).Value = item.User.Email;
                    worksheet.Cell(row, 3).Value = item.User.Birthdate.ToString();
                    worksheet.Cell(row, 4).Value = item?.User?.Mobile?.ToString();
                    worksheet.Cell(row, 5).Value = item?.Requestclients.FirstOrDefault()?.Street + ", " + item?.Requestclients.FirstOrDefault()?.City + ", " + item.Requestclients.FirstOrDefault()?.State + ", " + item.Requestclients.FirstOrDefault()?.Zipcode;

                    // Apply data body styling
                    var dataRange = worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 5));
                    var dataCellStyle = dataRange.Style;
                    dataCellStyle.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                    dataCellStyle.Font.FontColor = XLColor.FromHtml("#4F81BD");
                    row++;
                }
                else
                {
                    // Log the null reference or handle it appropriately
                    Console.WriteLine("Null reference detected in item: " + item);
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
        }
        catch (Exception e)
        {
            // Log the exception
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult ExportData(string status)
    {
        try
        {
            DashboardRequestQuery dashboardRequestQuery = new();
            string? StatusVal = string.IsNullOrEmpty(status) ? "new" : status;
            ViewBag.statusType = StatusVal;
            dashboardRequestQuery.Status = StatusVal;
            if (Request.Query["searchBy"] != "null") dashboardRequestQuery.SearchBy = Request.Query["searchBy"];
            if (Request.Query["region"] != "0") dashboardRequestQuery.Region = !string.IsNullOrEmpty(Request.Query["region"]) ? int.TryParse(Request.Query["region"], out int regionValue) ? regionValue : 0 : 0;
            dashboardRequestQuery.PageNumber = Request.Query.TryGetValue("pageNumber", out var pageNumberValue) ? int.Parse(pageNumberValue) : 1;
            dashboardRequestQuery.PageSize = Request.Query.TryGetValue("pageSize", out var pageSizeValue) ? int.Parse(pageSizeValue) : 5;
            ViewBag.currentPage = dashboardRequestQuery.PageNumber;
            ViewBag.currentPageSize = dashboardRequestQuery.PageSize;
            if (int.TryParse(Request.Query["requesttype"], out int reqType)) dashboardRequestQuery.RequestTypeId = reqType;
            AdminDashboardViewModel dashboardView = _adminDashboardService.GetDashboardRequests(dashboardRequestQuery);
            if (dashboardView.Requests == null)
            {
                TempData["error"] = "No data found";
                return BadRequest(new { message = "No data found" });
            }

            var memoryStream = ExportToExcel(dashboardView.Requests);

            // Return the Excel file as a downloadable file
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
        }
        catch (Exception e)
        {
            // Log the exception
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error";
            return BadRequest(new { message = "Exported Successfully" });
        }
    }

    private static MemoryStream ExportToExcel(List<RequestViewModel> requests)
    {
        try
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DataSheet1");

            // Set header text, style, and borders
            var headerStyle = worksheet.Range("A1:J1").Style;
            headerStyle.Font.Bold = true;
            headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
            headerStyle.Font.FontColor = XLColor.FromHtml("#DCE6F1");
            headerStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
            headerStyle.Border.BottomBorderColor = XLColor.Black;

            worksheet.Cell(1, 1).Value = "RequestId";
            worksheet.Cell(1, 2).Value = "Patient Name";
            worksheet.Column(2).Width = 20;
            worksheet.Cell(1, 3).Value = "Dob";
            worksheet.Column(3).Width = 15;
            worksheet.Cell(1, 4).Value = "Requestor";
            worksheet.Column(4).Width = 15;
            worksheet.Cell(1, 5).Value = "Requested Date";
            worksheet.Column(5).Width = 15;
            worksheet.Cell(1, 6).Value = "Patient Contact";
            worksheet.Column(6).Width = 15;
            worksheet.Cell(1, 7).Value = "Patient Address";
            worksheet.Column(7).Width = 45;
            worksheet.Cell(1, 8).Value = "Notes";
            worksheet.Column(8).Width = 40;
            worksheet.Cell(1, 9).Value = "Physician";
            worksheet.Column(9).Width = 15;
            worksheet.Cell(1, 10).Value = "Region";
            worksheet.Column(10).Width = 15;


            int row = 2;
            foreach (var item in requests)
            {
                // Check for null references in the item and related entities
                if (item != null)
                {
                    worksheet.Cell(row, 1).Value = item.Id;
                    worksheet.Cell(row, 2).Value = item.Firstname + " " + item.Lastname;
                    worksheet.Cell(row, 3).Value = item.BirthDate;
                    worksheet.Cell(row, 4).Value = item.Requestor;
                    worksheet.Cell(row, 5).Value = item.RequestedDate;
                    worksheet.Cell(row, 6).Value = item.Phonenumber;
                    worksheet.Cell(row, 7).Value = item.Address;
                    worksheet.Cell(row, 8).Value = item.Notes;
                    worksheet.Cell(row, 9).Value = item.PhysicianName;
                    worksheet.Cell(row, 10).Value = item.Region;

                    // Apply data body styling
                    var dataRange = worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 10));
                    var dataCellStyle = dataRange.Style;
                    dataCellStyle.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                    dataCellStyle.Font.FontColor = XLColor.FromHtml("#4F81BD");
                    row++;
                }
                else
                {
                    Console.WriteLine("Null reference detected in item: " + item);
                }
            }

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
        catch (Exception)
        {
            throw;
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
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRequest(PatientRequestViewModel newPatientRequest)
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
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    public async Task<IActionResult> LogOut()
    {
        try
        {    
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            string previousUrl = HttpContext.Session.GetString("PreviousUrl");
            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            else
            {
                TempData["error"] = "Internal Server Error";
                return RedirectToAction("Index", "Login");
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
