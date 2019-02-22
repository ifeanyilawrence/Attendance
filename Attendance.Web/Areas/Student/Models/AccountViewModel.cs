using Attendance.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Student.Models
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            ProgrammeSelectList = Utility.PopulateAllProgrammeSelectListItem();
            DepartmentSelectList = Utility.PopulateAllDepartmentSelectListItem();
            LevelSelectList = Utility.PopulateLevelSelectListItem();
            SessionSelectList = Utility.PopulateStaffSelectListItem();
            GenderSelectList = Utility.PopulateGenderSelectListItem();
            HallSelectList = Utility.PopulateHallSelectListItem();
        }
        public List<SelectListItem> ProgrammeSelectList { get; set; }
        public List<SelectListItem> DepartmentSelectList { get; set; }
        public List<SelectListItem> LevelSelectList { get; set; }
        public List<SelectListItem> SessionSelectList { get; set; }
        public List<SelectListItem> GenderSelectList { get; set; }
        public List<SelectListItem> HallSelectList { get; set; }
    }
}