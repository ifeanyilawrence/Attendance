using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class CourseLogic : BusinessBaseLogic<COURSE>
    {
        public List<COURSE> GetCoursesForStudent(STUDENT student)
        {
            List<COURSE> courses = null;
            try
            {
                StudentLevelLogic studentLevelLogic = new StudentLevelLogic();
                STUDENT_LEVEL studentLevel = studentLevelLogic.GetEntitiesBy(s => s.Student_Id == student.Person_Id).LastOrDefault();

                if (studentLevel != null)
                {
                    courses = base.GetEntitiesBy(c => c.Active && c.Department_Id == studentLevel.Department_Id && c.Level_Id == studentLevel.Level_Id && c.Programme_Id == studentLevel.Programme_Id);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return courses;
        }
    }
}
