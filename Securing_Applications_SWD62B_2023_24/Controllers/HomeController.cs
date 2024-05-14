﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Securing_Applications_SWD62B_2023_24.Helpers;
using Securing_Applications_SWD62B_2023_24.Models;
using System.Diagnostics;

namespace Securing_Applications_SWD62B_2023_24.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [CustomActionFilter]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles ="Admin, Moderator")]
        [Authorize()]
        [AuthorizeFileAccess()]
        public IActionResult Privacy()
        {
            return View();
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}