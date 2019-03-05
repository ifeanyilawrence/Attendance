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
    public class AttendanceLogic : BusinessBaseLogic<ATTENDANCE>
    {
        public void PopulateAttendanceForEvent(EVENT currentEvent)
        {
            try
            {
                StudentLevelLogic studentLevelLogic = new StudentLevelLogic();

                List<STUDENT_LEVEL> studentsInLevel = studentLevelLogic.GetEntitiesBy(s => s.Department_Id == currentEvent.Department_Id && s.Level_Id == currentEvent.Level_Id &&
                                                        s.Programme_Id == currentEvent.Programme_Id && s.Session_Id == currentEvent.Session_Id);
                studentsInLevel.ForEach(s =>
                {
                    if (base.GetEntitiesBy(a => a.Event_Id == currentEvent.Id && a.Student_Id == s.Student_Id).LastOrDefault() == null)
                    {
                        ATTENDANCE attendance = new ATTENDANCE();
                        attendance.Attendance_Status_Id = (int)AttendanceStatuses.Absent;
                        attendance.Cancelled = false;
                        attendance.Event_Id = currentEvent.Id;
                        attendance.Student_Id = s.Student_Id;
                        attendance.Time_Taken = currentEvent.Event_Start;

                        base.Create(attendance);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void MarkAttendance(STUDENT student, EVENT currentEvent, AttendanceStatuses status)
        {
            try
            {
                ATTENDANCE attendance = base.GetEntitiesBy(a => a.Student_Id == student.Person_Id && a.Event_Id == currentEvent.Id).LastOrDefault();
                if (attendance != null)
                {
                    attendance.Attendance_Status_Id = (int)status;
                    attendance.Time_Taken = DateTime.Now;

                    Modify(attendance);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Modify(ATTENDANCE attendance)
        {
            try
            {
                Expression<Func<ATTENDANCE, bool>> selector = a => a.Id == attendance.Id;
                ATTENDANCE entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Cancelled = attendance.Cancelled;
                    if (attendance.Attendance_Status_Id > 0)
                    {
                        entity.Attendance_Status_Id = attendance.Attendance_Status_Id;
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

        public List<ATTENDANCE> GetAttendanceForStudent(STUDENT student, DateTime date)
        {
            List<ATTENDANCE> attendanceList = null;
            try
            {
                attendanceList = base.GetEntitiesBy(a => a.Student_Id == student.Person_Id && a.EVENT.Date == date);
            }
            catch (Exception)
            {
                throw;
            }

            return attendanceList;
        }
    }
}
