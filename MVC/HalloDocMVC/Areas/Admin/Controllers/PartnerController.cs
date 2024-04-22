using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PartnerController : Controller
{
    private readonly IPartnerService _partnerService;

    public PartnerController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    public IActionResult Index(string VendorName = "", int ProfessionId = 0)
    {
        try
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PartnerTable", _partnerService.GetVendorList(VendorName, ProfessionId));
            }
            return View(_partnerService.GetVendorList("", 0));
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }

    }
    public IActionResult Create()
    {
        try
        {
            return View(_partnerService.GetAllRegionAndProfession());
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult Create(VendorDetail vendorInfo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(_partnerService.GetAllRegionAndProfession());
            }
            _partnerService.AddVendor(vendorInfo, false);
            TempData["Success"] = "Created Successfully!";
            return RedirectToAction("Create");
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Create");
        }

    }
    public IActionResult Delete(int Id)
    {
        try
        {
            _partnerService.DeleteVendor(Id);
            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {

            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    public IActionResult Edit(int Id)
    {
        try
        {
            return View(_partnerService.GetSingleBusiness(Id));
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult Edit(VendorDetail vendorInfo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Something went wrong! Please enter your details correct";
                return View(_partnerService.GetAllRegionAndProfession());
            }
            _partnerService.AddVendor(vendorInfo, true);
            TempData["Success"] = "Updated Successfully";
            return RedirectToAction("Edit");
        }
        catch (System.Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}