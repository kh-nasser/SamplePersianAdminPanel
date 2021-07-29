using PersianAdminPanel.Controllers;
using PersianAdminPanel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

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

        public override void Init()
        {
            base.AuthenticateRequest += OnAuthenticateRequest;
        }

        private void OnAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                var decodedTicket = FormsAuthentication.Decrypt(cookie.Value);
                var roles = decodedTicket.UserData.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                var principal = new GenericPrincipal(HttpContext.Current.User.Identity, roles);
                HttpContext.Current.User = principal;
            }
        }
    }
}