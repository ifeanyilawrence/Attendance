
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Attendance.Model.Entity;
using System.Linq.Expressions;
using Attendance.Data;

namespace Attendance.Business
{
    public class DepartmentLogic : BusinessBaseLogic<DEPARTMENT>
    {
        public bool Modify(DEPARTMENT department)
        {
            try
            {
                Expression<Func<DEPARTMENT, bool>> selector = f => f.Id == department.Id;
                DEPARTMENT entity = GetEntityBy(selector);

                if (entity == null)
                {
                    throw new Exception(NoItemFound);
                }

                entity.Name = department.Name;
                entity.Description = department.Description;

                int modifiedRecordCount = Save();
                if (modifiedRecordCount <= 0)
                {
                    throw new Exception(NoItemModified);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
