using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Attendance.Model.Entity;
using Attendance.Web.Models;

namespace Attendance.Web.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            RoleSelectList = Utility.PopulateRoleSelectListItem();
            MenuGroupSelectList = Utility.PopulateMenuGroupSelectListItem();
            MenuSelectList = Utility.PopulateMenuSelectListItem();
        }
        public ROLE Role { get; set; }
        public MENU_GROUP MenuGroup { get; set; }
        public MENU Menu { get; set; }
        public MENU_IN_ROLE MenuInRole { get; set; }
        public List<MENU> MenuList { get; set; }
        public List<MENU_IN_ROLE> MenuInRoleList { get; set; }
        public List<SelectListItem> RoleSelectList { get; set; }
        public List<SelectListItem> MenuGroupSelectList { get; set; }
        public List<SelectListItem> MenuSelectList { get; set; }
    }
}