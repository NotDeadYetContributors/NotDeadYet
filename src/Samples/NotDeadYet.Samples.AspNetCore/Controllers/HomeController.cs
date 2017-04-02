using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotDeadYet.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new ContentResult
            {
                Content = "Hello, world! You probably want to check out the /healthcheck endpoint.",
                ContentType = "text/plain"
            };
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
