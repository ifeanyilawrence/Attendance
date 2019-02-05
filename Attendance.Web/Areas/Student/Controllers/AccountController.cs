using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Student.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }
    }
}