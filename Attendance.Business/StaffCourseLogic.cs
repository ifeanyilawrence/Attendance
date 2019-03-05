using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class StaffCourseLogic : BusinessBaseLogic<STAFF_COURSE>
    {
        public bool Modify(STAFF_COURSE model)
        {
            try
            {
                Expression<Func<STAFF_COURSE, bool>> selector = a => a.Id == model.Id;
                STAFF_COURSE entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Staff_Id = model.Staff_Id;
                    entity.Course_Id = model.Course_Id;

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
