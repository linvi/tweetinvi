using System.Web.Mvc;

namespace Examplinvi.AccountActivity.ASP.NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
