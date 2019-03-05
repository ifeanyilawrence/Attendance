using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class StaffHallLogic : BusinessBaseLogic<STAFF_HALL>
    {
        public bool Modify(STAFF_HALL model)
        {
            try
            {
                Expression<Func<STAFF_HALL, bool>> selector = a => a.Id == model.Id;
                STAFF_HALL entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Staff_Id = model.Staff_Id;
                    entity.Hall_Id = model.Hall_Id;

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
