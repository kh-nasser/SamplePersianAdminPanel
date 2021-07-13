using System.Diagnostics;
using System.Web.Mvc;

namespace PersianAdminPanel.Controllers
{
    [HandleError]
    public class ManageErrorsController : Controller
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Logger.Logger logger = new Logger.Logger();
        // GET: ManageErrors
        public ActionResult Index()
        {
            return RedirectToAction("ErrorInternal", "ManageErrorsController");
            //return Redirect(Url.Content("~/"));
        }

        public ActionResult ErrorInternal()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}