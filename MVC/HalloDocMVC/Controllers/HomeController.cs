﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocMVC.Models;

namespace HalloDocMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SubmitRequest()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

      public IActionResult ForgotPassword()
    {
        return View();
    }
      public IActionResult PatientRequest()
    {
        return View();
    }

    public IActionResult FamilyRequest(){
        return View();
    }

    public IActionResult ConciergeRequest(){
        return View();
    }

    public IActionResult BusinessRequest(){
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
