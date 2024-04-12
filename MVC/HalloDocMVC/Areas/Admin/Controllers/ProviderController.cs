using Microsoft.AspNetCore.Mvc;
using HalloDocService.ViewModels;
using HalloDocService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocMVC.Services;
namespace HalloDocMVC.Controllers.Admin;
using Microsoft.AspNetCore.Hosting;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProviderController : Controller
{

    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IProviderService _providerService;
    private readonly IUtilityService _utilityService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IAccessService _accessService;


    public ProviderController(IAdminDashboardService adminDashboardService, IProviderService providerService, IUtilityService utilityService, IWebHostEnvironment hostingEnvironment, IAccessService accessService)
    {
        _adminDashboardService = adminDashboardService;
        _providerService = providerService;
        _utilityService = utilityService;
        _hostingEnvironment = hostingEnvironment;
        _accessService = accessService;
    }
    public async Task<IActionResult> Index(string regionId, string order)
    {
        try
        {

            AdminProviderViewModel providerViewModel = _providerService.GetAllProviderData(regionId, order);
            if (regionId != null && order != null)
            {
                ViewBag.Order = order;
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            else if (regionId != null)
            {
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            else if (order == "desc" || order == "asc")
            {
                ViewBag.Order = order;
                return PartialView("_ProviderListPartial", providerViewModel);
            }
            return View(providerViewModel);
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index", "Dashboard");
        }
    }
    [HttpPost]
    public IActionResult NotificationChange(AdminProviderViewModel viewModel)
    {
        try
        {
            List<string> stopNotificationIds = viewModel.StopNotificationIds;
            List<string> startNotificationIds = viewModel.StartNotificationIds;
            _providerService.UpdateNotification(stopNotificationIds, startNotificationIds);
            TempData["success"] = "Changes Are Saved Successfully";
            return Ok(new { message = "Updated Successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ContactProvider(string Id, string Communication, string Message)
    {
        try
        {
            var PhysicianData = _providerService.GetSingleProviderData(int.Parse(Id));
            _utilityService.EmailSend(null, "aakashdave21@gmail.com", Message, "Message From Admin");
            TempData["success"] = "Message Sent To Physician";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Edit(string? Id)
    {
        try
        {
            if (!string.IsNullOrEmpty(Id))
            {
                AdminPhysicianEditViewModel adminViewModel = _providerService.GetPhyisicianData(int.Parse(Id));
                if (!string.IsNullOrEmpty(adminViewModel.UploadSign))
                {
                    if (Path.IsPathRooted(adminViewModel.UploadSign))
                    {
                        adminViewModel.UploadSign = Path.Combine("uploads", Path.GetFileName(adminViewModel.UploadSign));
                    }
                }
                adminViewModel.UploadPhoto = !string.IsNullOrEmpty(adminViewModel.UploadPhoto) ?
                            Path.Combine("uploads", Path.GetFileName(adminViewModel.UploadPhoto)) :
                            adminViewModel.UploadPhoto;
                return View(adminViewModel);
            }
            return RedirectToAction("/Account/NotFound");

        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }

    }

    [HttpPost]
    public IActionResult SetPassword(string password, string Id)
    {
        try
        {
            string hashedPassword = PasswordHasher.HashPassword(password);
            _providerService.UpdateProviderPassword(int.Parse(Id), hashedPassword);
            return Ok(new { message = "Successfully Reset Password" });
        }
        catch (System.Exception e)
        {
            return BadRequest(new { message = e });
        }
    }

    [HttpPost]
    public IActionResult UpdatePersonalInfo(AdminPhysicianEditViewModel viewData)
    {
        try
        {
            _providerService.UpdatePersonalInformation(viewData);
            TempData["success"] = "Physician Updated Successfully";
            return Ok(new { message = "Successfully Updated" });
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return BadRequest(new { message = e });
        }
    }
    [HttpPost]
    public IActionResult UpdateGeneralInfo(AdminPhysicianEditViewModel viewData)
    {
        try
        {
            _providerService.UpdateGeneralInformation(viewData);
            TempData["success"] = "Physician Updated Successfully";
            return Ok(new { message = "Successfully Updated" });
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return BadRequest(new { message = e });
        }
    }
    [HttpPost]
    public IActionResult UpdateBillingInfo(AdminPhysicianEditViewModel viewData)
    {
        try
        {
            _providerService.UpdateBillingInfo(viewData);
            TempData["success"] = "Physician Updated Successfully";
            return Ok(new { message = "Successfully Updated" });
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return BadRequest(new { message = e });
        }
    }
    [HttpPost]
    public IActionResult BusinessSubmit(AdminPhysicianEditViewModel viewData, IFormFile userPhoto, IFormFile userSign, string ImageData)
    {
        try
        {
            AdminPhysicianEditViewModel updateData = new()
            {
                Businessname = viewData.Businessname,
                BusinessWebsite = viewData.BusinessWebsite,
                AdminNote = viewData.AdminNote,
                Id = viewData.Id
            };
            if (userSign == null && ImageData != null)
            {
                string? oldFilePath = _providerService.GetFilesPath(viewData.Id);
                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    RemoveFileIfExists(oldFilePath, "uploads");
                }
                updateData.UploadSign = ImageData;
            }
            if (userPhoto != null)
            {
                string? oldFilePath = _providerService.GetPhotoFilePath(viewData.Id);
                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    RemoveFileIfExists(oldFilePath, "uploads");
                }
                string photoPath = SaveFile(userPhoto, "uploads");
                updateData.UploadPhoto = photoPath;
            }
            if (userSign != null && ImageData == null)
            {
                string? oldFilePath = _providerService.GetFilesPath(viewData.Id);
                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    RemoveFileIfExists(oldFilePath, "uploads");
                }
                string signPath = SaveFile(userSign, "uploads");
                updateData.UploadSign = signPath;
            }
            _providerService.UpdateBusinessInformation(updateData);
            TempData["success"] = "Physician Updated Successfully";
            return Ok(new { message = "Successfully Updated" });
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return BadRequest(new { message = e });
        }
    }
    public string SaveFile(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0) return null;
        string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }
        return filePath;
    }
    public void RemoveFileIfExists(string fileName, string folderName)
    {
        string filePath = GetFilePath(fileName, folderName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
    private string GetFilePath(string fileName, string folderName)
    {
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
        return Path.Combine(uploadsFolder, fileName);
    }

    [HttpPost]
    public IActionResult UploadDoc(string Id, IFormFile File, string FileId)
    {
        try
        {
            string? oldFilePath = _providerService.GetAgreementFile(int.Parse(Id), FileId);
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                RemoveFileIfExists(oldFilePath, "uploads");
            }
            string filePath = SaveFile(File, "uploads");
            _providerService.UploadDocument(int.Parse(Id), FileId, filePath);
            TempData["success"] = "Document Uploaded Successfully";
            return Ok(new { message = "Successfully Updated" });
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return BadRequest(new { message = e });
        }
    }

    [HttpPost]
    public IActionResult DeleteProvider(string Id)
    {
        try
        {
            _providerService.DeleteProvider(int.Parse(Id));
            TempData["success"] = "Record Deleted Successfully";
            return RedirectToAction("Index");
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }

    public IActionResult Create()
    {
        try
        {
            return View(_providerService.GetRoleAndState());
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(AdminPhysicianCreateViewModel phyCreate, List<AllRegionList> AllCheckBoxRegionList)
    {
        try
        {
            if(!ModelState.IsValid){
                return View(_providerService.GetRoleAndState());
            }
            phyCreate.UploadPhoto = SaveFile(phyCreate.UserPhoto, "uploads");
            phyCreate.IsBgCheckFileName = phyCreate?.IsBgCheckFile != null ? SaveFile(phyCreate.IsBgCheckFile, "uploads") : null;
            phyCreate.IsHIPAAFileName = phyCreate?.IsHIPAAFile != null ? SaveFile(phyCreate.IsHIPAAFile, "uploads"): null;
            phyCreate.IsICAFileName = phyCreate?.IsICAFile != null ? SaveFile(phyCreate?.IsICAFile, "uploads"): null;
            phyCreate.IsNDAFileName = phyCreate?.IsNDAFile != null ? SaveFile(phyCreate?.IsNDAFile, "uploads"): null;
            phyCreate.Password = PasswordHasher.HashPassword(phyCreate.Password);
            phyCreate.AllCheckBoxRegionList = AllCheckBoxRegionList;
            _providerService.CreatePhysician(phyCreate);
            
            TempData["success"] = "Admin Created Successfully";
            return RedirectToAction("Create");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error !";
            return RedirectToAction("Index");
        }
    }

}