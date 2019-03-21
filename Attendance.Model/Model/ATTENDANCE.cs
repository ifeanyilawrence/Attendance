using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model.Entity
{
    public partial class ATTENDANCE
    {
        public double Percentage { get; set; }
        public bool IsEligible { get; set; }
    }
}
