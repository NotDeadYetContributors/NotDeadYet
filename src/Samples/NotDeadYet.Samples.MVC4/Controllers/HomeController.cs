using System.Web.Mvc;

namespace NotDeadYet.Samples.MVC4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult
                   {
                       Content = "Hello, world! You probably want to check out the /healthcheck endpoint.",
                       ContentType = "text/plain"
                   };
        }
    }
}