using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HalloDocService.Admin.Interfaces;
namespace HalloDocMVC.Controllers.Admin;
using System.Security.Claims;
using HalloDocService.Interfaces;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ChatController : Controller
{
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    public IActionResult Index()
    {
        try
        {
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            return View(_chatService.GetUserList(AspUserId));
        }
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    [HttpPost]
    public IActionResult CreateChat(int Id){
        try
        {
            int AspUserId = int.Parse(User.FindFirstValue("AspUserId"));
            _chatService.CreateChatUser(AspUserId, Id);
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    public IActionResult Connect(int Sender, int Receiver){
        try
        {
            return View(_chatService.LoadPreviousMessage(Sender,Receiver));
        }
        catch (Exception)
        {
            TempData["Error"] = "Internal Server Error";
            return RedirectToAction("Index");
        }
    }
}