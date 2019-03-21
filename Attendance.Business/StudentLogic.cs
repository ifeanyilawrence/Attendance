using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class StudentLogic : BusinessBaseLogic<STUDENT>
    {
        public bool Modify(STUDENT model)
        {
            try
            {
                Expression<Func<STUDENT, bool>> selector = a => a.Person_Id == model.Person_Id;
                STUDENT entity = GetEntityBy(selector);
                if (entity != null && entity.Person_Id > 0)
                {
                    entity.Matric_Number = model.Matric_Number;
                    entity.Ip_Address = model.Ip_Address;

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
