using Attendance.Model.Entity;
using Attendance.Model.Model;
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
        public List<AbsentLogModel> GetBy(LEVEL levelentity, COURSE courseentity)
        {
            try
            {
                List<AbsentLogModel> logs =
                    (from a in
                         repository.GetBy<VW_ABSENT_LOG>(
                             a =>
                                 a.Course_Id == courseentity.Id && a.Level_Id == levelentity.Id && a.Activated)
                     select new AbsentLogModel
                     {
                         Id = a.Id,
                         Name = a.Last_Name + " " + a.First_Name + a.Other_Name,
                         DepartmentId = a.Department_Id,
                         ProgrammeId = a.Programme_Id,
                         LevelId = a.Level_Id,
                         Duration = a.Duration_In_Days,
                         StartDate = a.Start_Date.ToLongDateString(),
                         EndDate = a.End_Date.ToLongDateString(),
                         Accept = (bool)a.Approved,
                         RejectReason = string.IsNullOrEmpty(a.Reject_Reason) ? " " : a.Reject_Reason,
                         Remark = string.IsNullOrEmpty(a.Remark) ? " " : a.Remark,
                         DepartmentName = a.Department_Name,
                         ProgrammeName = a.Programme_Name,
                         LevelName = a.Level_Name
                     }).ToList();
                return logs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetAbsenceRequestStatus(ATTENDANCE attendance)
        {
            string requestStatus = null;
            try
            {
                if (attendance != null)
                {
                    requestStatus = attendance.ATTENDANCE_STATUS.Name;

                    ABSENT_LOG absentLog = base.GetEntitiesBy(a => a.Event_Id == attendance.Event_Id && a.Student_Id == attendance.Student_Id).LastOrDefault();
                    if (absentLog != null && absentLog.Approved == true)
                        requestStatus += "(Request Approved)";
                    else if(absentLog != null && absentLog.Approved == false)
                        requestStatus += "(Request Reject)";
                    else if (absentLog != null && absentLog.Approved == null)
                        requestStatus += "(Pending)";
                    else
                        requestStatus += "(Pending)";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return requestStatus;
        }
    }
   
}
