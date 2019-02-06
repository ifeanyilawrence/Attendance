using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Attendance.Model.Model;

namespace Attendance.Web.Controllers
{
	public class BaseController : Controller
	{
		protected void SetMessage(string message, Message.Category messageType)
		{
			Message msg = new Message(message, (int)messageType);
			TempData["Message"] = msg;
		}
	}
}