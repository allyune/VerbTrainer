using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VerbTrainer.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/ - login page?
        public IActionResult Index()
        {
            return View();
        }
    }
}

