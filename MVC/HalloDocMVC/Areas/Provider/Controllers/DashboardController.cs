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
using HalloDocService.Provider.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text;

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


    public IActionResult Index(string status)
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
        catch (Exception e)
        {
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
            if (Type == "conclude")
            {
                return RedirectToAction("ConcludeCare", new { RequestId = requestId });
            }
            return RedirectToAction("ViewUploads", new { RequestId = requestId });
        }
        catch (Exception e)
        {
            TempData["error"] = "Internal Server Error";
            if (Type == "conclude")
            {
                return RedirectToAction("ConcludeCare", new { RequestId = requestId });
            }
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
            _utilityService.EmailSend("recipient@example.com", "Dear patient, please find attached the files you requested", "Set up your Account", filePaths, 3, reqId, null, null);
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
    public IActionResult TransferCase(int ReqId, string Description)
    {
        try
        {
            int PhysicianId = int.Parse(User.FindFirstValue("UserId"));
            _providerService.SetTransferCase(ReqId, PhysicianId, Description);
            TempData["success"] = "Request Transfered Successfully!";
            return Json(new { success = true, message = "Form data received successfully" });
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["error"] = "Internal Server Error";
            return Redirect("/Provider/Dashboard/Index");
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
            _adminDashboardService.CreateRequest(newPatientRequest, 2);
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
            string message = "Create Your Request here : <a href=\"" + CreateServiceLink + "\">Create Request</a>";
            _utilityService.EmailSend(rcvrMail, message, "Create Requests");
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
            if (isRequestFinalized == false)
            {
                TempData["error"] = "Please Finalize Medical Form Before Conclude Care!";
                return RedirectToAction("ConcludeCare", new { RequestId });
            }
            int? PhyId = int.Parse(User.FindFirstValue("UserId"));
            _providerService.ConcludeCare(RequestId, ProviderNote, PhyId);
            TempData["success"] = "Request Concluded";
            return RedirectToAction("ConcludeCare", new { RequestId });
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Finalized(int Id, int ReqId)
    {
        try
        {
            _providerService.FinalizeForm(Id, ReqId);
            TempData["success"] = "Form Finalized Successfully";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DownloadEncounter(int Id)
    {
        try
        {
            EncounterFormViewModel model = _adminDashboardService.GetEncounterDetails(Id);

            var pdf = new Document();
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdf, memoryStream);
                pdf.Open();


                var titleColor = new BaseColor(1, 188, 233);
                var titleFont = FontFactory.GetFont("Arial", 25, Font.BOLD, titleColor);
                var title = new Paragraph("Medical Confidential Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                pdf.Add(title);


                // Set the font
                var font = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);

                // Create a table
                var table = new PdfPTable(2); // 2 columns
                table.WidthPercentage = 100; // Full width
                table.SetWidths(new float[] { 1f, 2f }); // Relative column widths

                // Add header cells
                table.AddCell(new PdfPCell(new Phrase("Field", font)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Value", font)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                // Add data cells
                table.AddCell(new PdfPCell(new Phrase("First Name", font)));
                table.AddCell(new PdfPCell(new Phrase(model.FirstName, font)));

                table.AddCell(new PdfPCell(new Phrase("Last Name", font)));
                table.AddCell(new PdfPCell(new Phrase(model.LastName, font)));

                table.AddCell(new PdfPCell(new Phrase("DOB", font)));
                table.AddCell(new PdfPCell(new Phrase(model.DateOfBirth.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("Mobile", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Mobile, font)));

                table.AddCell(new PdfPCell(new Phrase("Email", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Email, font)));

                table.AddCell(new PdfPCell(new Phrase("Location", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Location, font)));

                table.AddCell(new PdfPCell(new Phrase("History Of Illness", font)));
                table.AddCell(new PdfPCell(new Phrase(model.HistoryOfPresentIllness, font)));

                table.AddCell(new PdfPCell(new Phrase("Medical History", font)));
                table.AddCell(new PdfPCell(new Phrase(model.MedicalHistory, font)));

                table.AddCell(new PdfPCell(new Phrase("Medication", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Medications, font)));

                table.AddCell(new PdfPCell(new Phrase("Allergies", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Allergies, font)));

                table.AddCell(new PdfPCell(new Phrase("Temp", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Temperature.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("HR", font)));
                table.AddCell(new PdfPCell(new Phrase(model.HeartRate.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("RR", font)));
                table.AddCell(new PdfPCell(new Phrase(model.RespiratoryRate.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("BPs", font)));
                table.AddCell(new PdfPCell(new Phrase(model.BloodPressureSBP.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("BPd", font)));
                table.AddCell(new PdfPCell(new Phrase(model.BloodPressureDBP.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("O2", font)));
                table.AddCell(new PdfPCell(new Phrase(model.O2.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("Pain", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Pain.ToString(), font)));

                table.AddCell(new PdfPCell(new Phrase("Heent", font)));
                table.AddCell(new PdfPCell(new Phrase(model.HEENT, font)));

                table.AddCell(new PdfPCell(new Phrase("CV", font)));
                table.AddCell(new PdfPCell(new Phrase(model.CV, font)));

                table.AddCell(new PdfPCell(new Phrase("Chest", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Chest, font)));

                table.AddCell(new PdfPCell(new Phrase("ABD", font)));
                table.AddCell(new PdfPCell(new Phrase(model.ABD, font)));

                table.AddCell(new PdfPCell(new Phrase("Extr", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Extr, font)));

                table.AddCell(new PdfPCell(new Phrase("Skin", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Skin, font)));

                table.AddCell(new PdfPCell(new Phrase("Neuro", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Neuro, font)));

                table.AddCell(new PdfPCell(new Phrase("Other", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Other, font)));

                table.AddCell(new PdfPCell(new Phrase("Diagnosis", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Diagnosis, font)));

                table.AddCell(new PdfPCell(new Phrase("Treatment Plan", font)));
                table.AddCell(new PdfPCell(new Phrase(model.TreatmentPlan, font)));

                table.AddCell(new PdfPCell(new Phrase("Medications Dispended", font)));
                table.AddCell(new PdfPCell(new Phrase(model.MedicationDispensed, font)));

                table.AddCell(new PdfPCell(new Phrase("Procedure", font)));
                table.AddCell(new PdfPCell(new Phrase(model.Procedures, font)));

                table.AddCell(new PdfPCell(new Phrase("FollowUp", font)));
                table.AddCell(new PdfPCell(new Phrase(model.FollowUp, font)));

                table.AddCell(new PdfPCell(new Phrase("Is Finalized", font)));
                table.AddCell(new PdfPCell(new Phrase(model.IsFinalized.ToString(), font)));

                // Add the table to the document
                pdf.Add(table);

                pdf.Close();
                writer.Close();

                var bytes = memoryStream.ToArray();
                var result = new FileContentResult(bytes, "application/pdf");
                result.FileDownloadName = "Encounter_" + model.Id + ".pdf";
                return result;
            }
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }

    public IActionResult SendProfileRequest()
    {
        try
        {
            TempData["success"] = "Request Sent Successfully!";
            return RedirectToAction("Index");

        }
        catch (System.Exception e)
        {
            TempData["error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }


    public async Task<IActionResult> LogOut()
    {

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        return RedirectToAction("Index", "Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
