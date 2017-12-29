using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForEvolve.AspNetCore.Localization.Models;

namespace ForEvolve.AspNetCore.Localization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var viewModel = new ContactViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            return View("ContactResult", viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
