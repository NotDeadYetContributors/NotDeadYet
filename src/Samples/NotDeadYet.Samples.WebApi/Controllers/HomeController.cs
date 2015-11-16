using System.Web.Mvc;

namespace NotDeadYet.Samples.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello, world! You probably want to check out the /healthcheck endpoint.");
        }
    }
}