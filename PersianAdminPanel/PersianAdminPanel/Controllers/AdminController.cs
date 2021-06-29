using System.Web.Mvc;

namespace PersianAdminPanel.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
    }
}