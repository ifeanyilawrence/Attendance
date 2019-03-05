using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        
        public bool Modify(COURSE model)
        {
            try
            {
                Expression<Func<COURSE, bool>> selector = a => a.Id == model.Id;
                COURSE entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Code = model.Code;
                    entity.Name = model.Name;
                    if (model.Programme_Id > 0)
                    {
                        entity.Programme_Id = model.Programme_Id;
                    }
                    if (model.Department_Id > 0)
                    {
                        entity.Department_Id = model.Department_Id;
                    }
                    if (model.Level_Id > 0)
                    {
                        entity.Level_Id = model.Level_Id;
                    }

                    int modifiedRecordCount = Save();

                    if (modifiedRecordCount > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
