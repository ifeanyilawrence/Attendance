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
    }
}