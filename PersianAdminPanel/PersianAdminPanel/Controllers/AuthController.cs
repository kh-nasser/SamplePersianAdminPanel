using Common.DataModel.Domain.Models;
using Common.DataModel.DTO.Communication;
using PersianAdminPanel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersianAdminPanel.Controllers
{
    [HandleError]
    public class AuthController : Controller
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Logger.Logger logger = new Logger.Logger();
        private readonly BusinessLogic.Client.Authorization.Authorization _authorization = new BusinessLogic.Client.Authorization.Authorization();

        // GET: Auth
        [AllowAnonymous]
        public ActionResult Signin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Signin");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(UserSignin user)
        {
            try
            {
                stopwatch.Start();

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (Session["Captcha"] == null || Session["Captcha"].ToString() != user.Captcha)
                {
                    ModelState.AddModelError("Captcha", "entered sum value did not match, please try again");
                }
                //else if (!ModelState.IsValid)
                //{
                //    ViewBag.Message = string.Join("; ", ModelState.Values
                //                        .SelectMany(x => x.Errors)
                //                        .Select(x => x.ErrorMessage));

                //}
                else
                {
                    var result = _authorization.Signin(user);
                    if (result.HasError())
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        //result.Resource.TryGetValue("exp", out string expStr);
                        //var exp = DateTime.Parse(expStr);
                        //CookieUtils.StoreInCookie("auth", null, result.Resource, exp);
                        //FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                        // Roles.AddUserToRole(model.UserName, UserRole);
                        CreateTicket(user, result);
                        return RedirectToAction("Dashboard", "Admin");
                        //ViewBag.account = account;
                    }
                }

                stopwatch.Stop();
                user.Password = "";
                logger.Verbose(duration: stopwatch.ElapsedMilliseconds, response: user, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });

                return View(user);
            }
            catch (Exception ex)
            {
                logger.Fatal(exception: ex, null, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });
                throw ex;
            }
        }

        private void CreateTicket(UserSignin user, BaseResponse<Dictionary<string, string>> result)
        {
            // Roles.AddUserToRole(model.UserName, UserRole);
            result.Resource.TryGetValue("exp", out string expStr);
            var exp = DateTime.Parse(expStr);

            result.Resource.TryGetValue("RoleId", out string roleStr);

            var ticket = new FormsAuthenticationTicket(
                  version: 1
                  , name: user.Username
                  , issueDate: DateTime.Now
                  , expiration: exp//DateTime.Now.AddSeconds(HttpContext.Session.Timeout)
                  , isPersistent: user.RememberMe
                  , userData: String.Join("|", roleStr));

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            HttpContext.Response.Cookies.Add(cookie);


            //CookieUtils.StoreInCookie("auth", null, result.Resource, exp);
            //FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
            //return RedirectToAction("Dashboard", "Admin");
            //ViewBag.account = account;

        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(UserSignup user)
        {
            try
            {
                stopwatch.Start();
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (Session["Captcha"] == null || Session["Captcha"].ToString() != user.Captcha)
                {
                    ModelState.AddModelError("Captcha", "entered sum value did not match, please try again");
                }
                else if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                    ViewBag.Message = errors[0];

                }
                else
                {
                    var result = _authorization.Signup(user);
                    if (result.HasError())
                    {
                        ViewBag.Message = result.Message;
                    }
                }
                stopwatch.Stop();
                user.Password = "";
                logger.Verbose(duration: stopwatch.ElapsedMilliseconds, response: user, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });

                return View(user);
            }
            catch (Exception ex)
            {
                logger.Fatal(exception: ex, null, request: new object[] { IPAddressHelper.GetClientIpAddress(Request), user });
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            CookieUtils.RemoveCookie("auth");

            if (Session != null)
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
            }
            return RedirectToAction("SignIn", "Auth");
        }
    }
}