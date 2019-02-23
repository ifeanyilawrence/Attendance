using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model.Model
{
    public class JsonResponseModel
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public int NumberOfPresent { get; set; }
        public int TotalNumberOfLectures { get; set; }
        public int ApproximateNumberOfLectures { get; set; }
        public int NumberOfAbsent { get; set; }
        public double EligibilityPercentage { get; set; }
        public bool IsEligible { get; set; }
    }
}
