using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance.Web.Areas.Student.Models
{
    public class AttendanceViewModel
    {
        public List<EVENT> Events { get; set; }
        public List<ATTENDANCE> AttendanceList { get; set; }
        public List<ABSENT_LOG> AbsenceList { get; set; }
        public string Date { get; set; }
    }
}