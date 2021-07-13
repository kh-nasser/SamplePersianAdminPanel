using Logger;
using System.Diagnostics;
using System.Web.Mvc;

namespace PersianAdminPanel.Controllers
{
    [Authorize]
    [HandleError]
    public class AdminController : Controller
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Logger.Logger logger = new Logger.Logger();
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