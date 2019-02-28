using Attendance.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Admin.Models
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            GenderSelectList = Utility.PopulateGenderSelectListItem();
            RoleSelectList = Utility.PopulateRoleSelectListItem();
            StaffTypeSelectList = Utility.PopulateStaffTypeSelectListItem();
        }
        public List<SelectListItem> GenderSelectList { get;  set; }
        public List<SelectListItem> RoleSelectList { get; set; }
        public List<SelectListItem> StaffTypeSelectList { get; set; }

    }
    public class SignupJsonModel
    {
        public string surname { get; set; }
        public string firstname { get; set; }
        public string othernames { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public string role { get; set; }

        public string staffType { get; set; }
        public string regnumber { get; set; }
    }

}