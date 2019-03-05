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
        public long CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int ProgrammeId { get; set; }
        public int DepartmentId { get; set; }
        public int LevelId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int HallId { get; set; }
        public string HallName { get; set; }
        public string HallDescription { get; set; }
    }
}
