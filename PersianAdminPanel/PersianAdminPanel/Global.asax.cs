using PersianAdminPanel.Controllers;
using PersianAdminPanel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PersianAdminPanel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly BusinessLogic.Client.Authorization.Authorization _authorization = new BusinessLogic.Client.Authorization.Authorization();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        void Session_Start(object sender, EventArgs e)
        {
            bool authorized = false;
            bool userNameExist = CookieUtils.CookieExist("auth", "Username");
            bool userTokenExist = CookieUtils.CookieExist("auth", "Token");
            if (userNameExist && userTokenExist)
            {
                string userName = CookieUtils.GetFromCookie("auth", "Username");
                Guid userToken = Guid.Parse(CookieUtils.GetFromCookie("auth", "Token"));
                long.TryParse(CookieUtils.GetFromCookie("auth", "UserId"), out long userId);
                if (_authorization.ValidateUserToken(userId, userToken))
                {
                    authorized = true;
                }
                if (authorized)
                {
                    HttpContext.Current.Session.Add("__auth", new KeyValuePair<string, long>(userName, userId));
                }
                else
                {
                    new AuthController().Logout();
                }
            }
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //log an exception
        }
    }
}