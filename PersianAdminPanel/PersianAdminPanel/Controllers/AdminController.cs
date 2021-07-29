using PersianAdminPanel.Models;
using PersianAdminPanel.Utils;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace PersianAdminPanel.Controllers
{
    [HandleError]
    [Authorize]
    [AuthorizeRoles(CustomRoles.SuperAdmin, CustomRoles.ADMIN)]
    public class AdminController : Controller
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Logger.Logger logger = new Logger.Logger();
        private readonly BusinessLogic.Client.Authorization.Authorization _authorization = new BusinessLogic.Client.Authorization.Authorization();
        private readonly BissinessLogic.Client.RegisteredUserBLL _registeredUserBLL = new BissinessLogic.Client.RegisteredUserBLL();

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        //public ActionResult UserApproval() { }
        public ActionResult RegisteredUser()
        {
            var user = User.Identity.Name;
            try
            {
                stopwatch.Start();

                var result = _registeredUserBLL.GetAll();

                stopwatch.Stop();

                logger.Verbose(duration: stopwatch.ElapsedMilliseconds, response: user, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });

                return View(result);
            }
            catch (Exception ex)
            {
                logger.Fatal(exception: ex, null, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });
                throw ex;
            }
        }
    }
}

/**
 *  var data = _Dashboard.GET_CLIENT_BANKS(RegisteredUser.Identity.Name, pageSize, page ?? 1);
            IPagedList pagedData = data.ToPagedList(page ?? 1, pageSize);
            return View(pagedData);




    </div>

            @Html.PagedListPager(Model, page => Url.Action("UserTransactions", new
       {
           page
           //,sortOrder = ViewBag.CurrentSort
           //,currentFilter = viewBag.CurrentFilter
       }),

       new PagedListRenderOptions()
       {

           DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded,
           DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded,
           DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded,
           DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded,
           DisplayLinkToIndividualPages = true,
           DisplayPageCountAndCurrentLocation = false,
           MaximumPageNumbersToDisplay = 10,
           DisplayEllipsesWhenNotShowingAllPageNumbers = true,
           EllipsesFormat = "&#8230;",
           LinkToFirstPageFormat = "««",
           LinkToPreviousPageFormat = "«",//"simple-icon-arrow-right"
           LinkToIndividualPageFormat = "{0}",
           LinkToNextPageFormat = "»",//"simple-icon-arrow-right"
           LinkToLastPageFormat = "»»",
           //PageCountAndCurrentLocationFormat = "صفحه {0} از {1} ",
           //ItemSliceAndTotalFormat = " {2} نمایش ایتم های {0} از {1} و ",
           //DelimiterBetweenPageNumbers = "|",
           FunctionToDisplayEachPageNumber = null,
           ClassToApplyToFirstListItemInPager = "page-link first",
           ClassToApplyToLastListItemInPager = "page-link last",
           ContainerDivClasses = new[] { "mt-4 mb-3" },
           UlElementClasses = new[] { "pagination justify-content-center mb-0" },
           LiElementClasses = new List<string> { "page-item" }
       }
       )
        </div>
    </div>
 */
