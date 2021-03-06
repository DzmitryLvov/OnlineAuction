﻿using System;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Account;
namespace OnlineAuction.Buisness.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            //Session.Add("ToRedirect", );
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var createStatus = new DataAccess().AddNewUser(model);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }


            // если мы оказались тут, чтото пошло не так, заново отобразить форму
            return View(model);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(User.Identity.Name, model.OldPassword))
                {
                    Membership.Provider.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Password updating fail.");

            return View(model);
        }
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        public ActionResult EmailIsSent()
        {
            return View();
        }

        [Authorize]
        public ActionResult Profile(string username)
        {
            if(username == null) username = HttpContext.User.Identity.Name;

            return View(new DataAccess().GetUserProfileViewModel(username));
        }


        [Authorize]
        public ActionResult EditProfile()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult SetAsAdministrator( string username)
        {
            new DataAccess().SetAdmin(username);
            return RedirectToAction("Profile", new {username = username});
        }

        public JsonResult GetLocations()
        {
            return Json(new DataAccess().GetLocations(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BanUser(string username)
        {
            new DataAccess().BanUser(username);
            return RedirectToAction("Profile", new { username = username });
        }

        public ActionResult UnBanUser(string username)
        {
            new DataAccess().UnBanUser(username);
            return RedirectToAction("Profile", new { username = username });
        }

        [Authorize]
        public ActionResult Bookmarks()
        {
            return View();
        }
        public ActionResult GetBookmarks([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetUserBookmarks(HttpContext.User.Identity.Name).ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUser(string username)
        {
            new DataAccess().DeleteUser(username);
             return RedirectToAction("Index","Home");
        }
    }
}