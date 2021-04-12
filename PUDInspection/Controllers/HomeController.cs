using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PUDInspection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PUDInspection.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;

namespace PUDInspection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("creator"))
                return RedirectToAction("ListRoles", "Administration");
            if (User.IsInRole("admin") || User.IsInRole("moderator"))
                return RedirectToAction("Index", "InspectionSpaces");
            if (User.IsInRole("inspector"))
                return RedirectToAction("Index", "Inspector");
            if (User.IsInRole("validator"))
                return RedirectToAction("Index", "Validator");

            return RedirectToAction("Privacy");
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
