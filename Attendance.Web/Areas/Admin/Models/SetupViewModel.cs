using Attendance.Model.Entity;
using Attendance.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Admin.Models
{
    public class SetupViewModel
    {
        public SetupViewModel()
        {
            LecturerSelectList = Utility.PopulateLecturerSelectListItem();
            HallStaffSelectList = Utility.PopulateHallStaffSelectListItem();
            CourseSelectList = Utility.PopulateCourseSelectListItem();
            HallSelectList = Utility.PopulateHallSelectListItem();
            ProgrammeSelectList = Utility.PopulateAllProgrammeSelectListItem();
            DepartmentSelectList = Utility.PopulateAllDepartmentSelectListItem();
            LevelSelectList = Utility.PopulateLevelSelectListItem();
        }
        public List<STAFF_HALL> StaffHallList { get; set; }
        public List<SelectListItem> LecturerSelectList { get; set; }
        public List<SelectListItem> HallStaffSelectList { get; set; }
        public List<SelectListItem> CourseSelectList { get; set; }
        public List<SelectListItem> HallSelectList { get; set; }
        public STAFF_HALL StaffHall { get; set; }
        public STAFF_COURSE StaffCourse { get; set; }
        public List<STAFF_COURSE> StaffCourseList { get; set; }
        public List<COURSE> CourseList { get; set; }
        public List<HALL> HallList { get; set; }
        public List<LOCATION> LocationList { get; set; }
        public List<SelectListItem> ProgrammeSelectList { get; set; }
        public List<SelectListItem> DepartmentSelectList { get; set; }
        public List<SelectListItem> LevelSelectList { get; set; }
    }
}