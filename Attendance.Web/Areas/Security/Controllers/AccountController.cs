using Attendance.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Attendance.Web.Areas.Security.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [AllowAnonymous]
        public JsonResult Login(string username, string password)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                if (userLogic.ValidateUser(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, false);

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Invalid username or password.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Clear();
            return RedirectToAction("Login", "Account", new { Area = "Security" });
        }
        public ActionResult Home()
        {
            return View();
        }
    }
}