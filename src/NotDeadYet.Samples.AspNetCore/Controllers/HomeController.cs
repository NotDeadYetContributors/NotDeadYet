using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace NotDeadYet.Samples.AspNetCore.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new ContentResult
            {
                Content = "Hello, world! You probably want to check out the /healthcheck endpoint.",
                ContentType = "text/plain"
            };
        }
    }
}
