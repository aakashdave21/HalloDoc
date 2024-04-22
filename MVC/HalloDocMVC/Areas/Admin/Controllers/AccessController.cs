using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HalloDocService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
using HalloDocMVC.Services;
namespace HalloDocMVC.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AccessController : Controller
{
    private readonly IAccessService _accessService;

    public AccessController(IAccessService accessService)
    {
        _accessService = accessService;
    }

    public IActionResult Index()
    {
        try
        {
            return View(_accessService.GetAllAccessList());
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    [HttpPost]
    public IActionResult Delete(string Id)
    {
        try
        {
            _accessService.DeleteAccess(int.Parse(Id));
            TempData["success"] = "Record Deleted Successfully";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index");
        }
    }

    public IActionResult Create()
    {
        try
        {
            AdminAccessViewModel accessView = new()
            {
                AccessMenuList = _accessService.GetMenuList(0)
            };
            return View(accessView);
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult Create(IFormCollection formData)
    {
        try
        {   
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            string? roleName = formData?["rolename"];
            int AccountType = int.Parse(formData?["accountType"]);
            List<int> MenuArray = formData["menus"].Select(int.Parse).ToList();
            _accessService.CreateNewRole(roleName,AccountType,MenuArray,AspUserId);
            TempData["success"] = "Role Created Successfully";
           return RedirectToAction("Create");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error!";
             return RedirectToAction("Create");
        }
    }

    [HttpPost]
    public IActionResult GetMenuByAccountType(string accountId,string? roleId=null)
    {
        try
        {
            AdminAccessViewModel accessView = new()
            {
                AccessMenuList = _accessService.GetMenuList(int.Parse(accountId)),
                SelectedIds = roleId != null ? _accessService.GetCheckedMenu(int.Parse(roleId)) : new List<int>()
            };
            foreach (var item in _accessService.GetCheckedMenu(int.Parse(accountId)))
            {
                Console.WriteLine(item);
            }
            return PartialView("_MenuList", accessView);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });

        }
    }

    [HttpPost]
    public IActionResult Edit(string Id)
    {
        try
        {
            AdminAccessEditViewModel accessView = _accessService.GetEditViewData(int.Parse(Id));
            return View(accessView);
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult EditPost(AdminAccessEditViewModel viewData,string SelectedMenus, string UnselectedMenus)
    {
        try
        {

            List<int> selectedMenusList = !string.IsNullOrEmpty(SelectedMenus) ? SelectedMenus.Split(',').Select(int.Parse).ToList() : new List<int>();
            List<int> unSelectedMenusList = !string.IsNullOrEmpty(UnselectedMenus) ? UnselectedMenus.Split(',').Select(int.Parse).ToList() : new List<int>();
            _accessService.UpdateRoleData(viewData,selectedMenusList,unSelectedMenusList);
            TempData["success"] = "Role Updated Successfully !";
            return RedirectToAction("Index");
        }
        catch (System.Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index");
        }
    }

    public IActionResult Account(){
        try
        {
            return View( _accessService.GetRoleAndState());
        }
        catch (Exception)
        {
            TempData["error"] = "Internal Server Error!";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult Account(AdminAccountViewModel adminData){
        try
        {
            if (!ModelState.IsValid)
            {
                AdminAccountViewModel adminRoleAndState =  _accessService.GetRoleAndState();
                adminData.RoleId = adminData.RoleId;
                adminData.State = adminData.State;
                adminData.AllRoleList = adminRoleAndState.AllRoleList;
                adminData.AllRegionsList = adminRoleAndState.AllRegionsList;
                adminData.AllCheckBoxRegionList = adminRoleAndState.AllCheckBoxRegionList;
                return View(nameof(Account), adminData);
            }
            adminData.CreatedUser = int.Parse(User.FindFirstValue("AspUserId"));
            if(adminData.Password != null){
                adminData.Password = PasswordHasher.HashPassword(adminData.Password);
            }
            _accessService.CreateAdminAccount(adminData);
            TempData["success"] = "Admin Created Successfully !";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            if (e.Message == "EmailAlreadyExists")
            {
                TempData["error"] = "The email address already exists!";
                return View(nameof(Account), adminData);
            }
            else
            {
                TempData["error"] = "Internal Server Error!";
            }
            return RedirectToAction("Index");
        }
    }

    

}