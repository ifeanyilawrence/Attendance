using Attendance.Model.Entity;
using Attendance.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Admin.Models
{
    public class EventViewModel
    {
        public EventViewModel()
        {
            LecturerSelectList = Utility.PopulateLecturerSelectListItem();
            HallStaffSelectList = Utility.PopulateHallStaffSelectListItem();
            CourseSelectList = Utility.PopulateCourseSelectListItem();
            HallSelectList = Utility.PopulateHallSelectListItem();
            ProgrammeSelectList = Utility.PopulateAllProgrammeSelectListItem();
            DepartmentSelectList = Utility.PopulateAllDepartmentSelectListItem();
            LevelSelectList = Utility.PopulateLevelSelectListItem();
            EventTypeSelectList = Utility.PopulateEventTypeSelectListItem();
            SessionSelectList = Utility.PopulateSessionSelectListItem();
            LocationSelectList = Utility.PopulateLocationSelectListItem();
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
        public EVENT Event { get; set; }
        public List<EVENT> Events { get; set; }
        public List<ATTENDANCE> AttendanceList { get; set; }
        public List<SelectListItem> EventTypeSelectList { get; set; }
        public List<SelectListItem> SessionSelectList { get; set; }
        public List<SelectListItem> LocationSelectList { get; set; }
        public string DateString { get; set; }
        public EVENT_TYPE EventType { get; set; }
        public long EventId { get; set; }
        public COURSE Course { get; set; }
        public SESSION Session { get; set; }
        public PROGRAMME Programme { get; set; }
        public DEPARTMENT Department { get; set; }
        public LEVEL Level { get; set; }
        public HALL Hall { get; set; }
    }
    public class EventModel
    {
        public string CourseId { get; set; }
        public string ProgrammeId { get; set; }
        public string DepartmentId { get; set; }
        public string LevelId { get; set; }
        public string LocationId { get; set; }
        public string HallId { get; set; }
        public string EventId { get; set; }
        public string EventTypeId { get; set; }
        public string EventStart { get; set; }
        public string EventEnd { get; set; }
        public string Date { get; set; }
        public string SessionId { get; set; }
        public string EventActive { get; set; }
        public string IsWeekly { get; set; }
    }
    public class AttendanceModel
    {
        public int SN { get; set; }
        public string Name { get; set; }
        public string Registration_Number { get; set; }
        public string Event_Type { get; set; }
        public string Course_Hall { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Time_Taken { get; set; }
        public string Status { get; set; }
    }
    public class SemesterAttendanceModel
    {
        public int SN { get; set; }
        public string Name { get; set; }
        public string Registration_Number { get; set; }
        public string Event_Type { get; set; }
        public string Course_Hall { get; set; }
        public string Percentage { get; set; }
        public string Status { get; set; }
    }
}