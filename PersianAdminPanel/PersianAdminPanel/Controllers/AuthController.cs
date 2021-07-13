using PersianAdminPanel.Models;
using PersianAdminPanel.Utils;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace PersianAdminPanel.Controllers
{
    public class AuthController : Controller
    {
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                if (Session["Captcha"] == null || Session["Captcha"].ToString() != user.Captcha)
                {
                    ModelState.AddModelError("Captcha", "entered sum value did not match, please try again");
                }
                //else if (!ModelState.IsValid)
                //{
                //    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                //}
                else
                {

                    using (var usersEntities = new SampleLoginDbEntities())
                    {
                        int? userId = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();

                        string message = string.Empty;
                        switch (userId.Value)
                        {
                            case -1:
                                message = "Username and/or password is incorrect.";
                                break;
                            case -2:
                                message = "Account has not been activated.";
                                break;
                            default:
                                {
                                    var dict = new System.Collections.Generic.Dictionary<string, string>();
                                    Guid userToken = usersEntities.UserActivations.Where(c => c.UserId == userId.Value).Select(x => x.ActivationCode).First();

                                    dict.Add("issued", DateTime.Now.ToString());
                                    DateTime expDate = DateTime.Now.AddYears(1);
                                    dict.Add("exp", expDate.ToString());
                                    dict.Add("Username", user.Username);
                                    dict.Add("UserId", userId.Value.ToString());
                                    dict.Add("Token", userToken.ToString());
                                    CookieUtils.StoreInCookie("auth", null, dict, expDate);
                                    FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                                    return RedirectToAction("Dashboard", "Admin");
                                }
                        }

                        //ViewBag.account = account;
                        ViewBag.Message = message;
                    }

                }
            }
            return View(user);
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                using (var usersEntities = new SampleLoginDbEntities())
                {
                    usersEntities.Users.Add(new User()
                    {
                        CreatedDate = user.CreatedDate,
                        Email = user.Email,
                        LastLoginDate = user.LastLoginDate,
                        Password = user.Password,
                        UserId = user.UserId,
                        Username = user.Username,
                    });

                    usersEntities.SaveChanges();
                    string message = string.Empty;
                    switch (user.UserId)
                    {
                        case -1:
                            message = "Username already exists.\\nPlease choose a different username.";
                            break;
                        case -2:
                            message = "Supplied email address has already been used.";
                            break;
                        default:
                            message = "Registration successful.\\nUser Id: " + user.UserId.ToString();
                            break;
                    }
                    ViewBag.Message = message;
                }
                return View(user);
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