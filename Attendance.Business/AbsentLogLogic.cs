using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class AbsentLogLogic : BusinessBaseLogic<ABSENT_LOG>
    {
        public bool Modify(ABSENT_LOG absenceLog)
        {
            try
            {
                Expression<Func<ABSENT_LOG, bool>> selector = a => a.Id == absenceLog.Id;
                ABSENT_LOG entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Approved = absenceLog.Approved;
                    entity.Duration_In_Days = absenceLog.Duration_In_Days;
                    entity.End_Date = absenceLog.End_Date;
                    entity.Reject_Reason = absenceLog.Reject_Reason;
                    entity.Remark = absenceLog.Remark;
                    entity.Start_Date = absenceLog.Start_Date;
                    
                    if (absenceLog.Absent_Type_Id > 0)
                    {
                        entity.Absent_Type_Id = absenceLog.Absent_Type_Id;
                    }
                    if (absenceLog.User_Id > 0)
                    {
                        entity.User_Id = absenceLog.User_Id;
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
