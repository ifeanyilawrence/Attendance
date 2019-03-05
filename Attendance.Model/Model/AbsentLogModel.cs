using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model.Model
{
    public class AbsentLogModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public int ProgrammeId { get; set; }
        public int DepartmentId { get; set; }
        public int LevelId { get; set; }
        public string ProgrammeName { get; set; }
        public string DepartmentName { get; set; }
        public string LevelName { get; set; }
        public int? Duration { get; set; }
        public bool Accept { get; set; }
        public bool Decline { get; set; }
        public string Remark { get; set; }
        public string RejectReason { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
