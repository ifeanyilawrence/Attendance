using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Student.Models;
using Attendance.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Student.Controllers
{
    public class AttendanceController : BaseController
    {
        private AttendanceViewModel _viewModel;
        private STUDENT _student;

        private EventLogic _eventLogic;
        private StudentLogic _studentLogic;
        private AbsentTypeLogic _absentTypeLogic;
        private AttendanceLogic _attendanceLogic;
        private AbsentLogLogic _absentLogLogic;
        public AttendanceController()
        {
            _studentLogic = new StudentLogic();
            _eventLogic = new EventLogic();
            _absentTypeLogic = new AbsentTypeLogic();
            _attendanceLogic = new AttendanceLogic();
            _absentLogLogic = new AbsentLogLogic();
        }
        public ActionResult ViewCurrentEvents()
        {
            _viewModel = new AttendanceViewModel();
            ViewBag.AbsentType = _absentTypeLogic.GetEntitiesBy(a => a.Active);
            try
            {
                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                _viewModel.Events = _eventLogic.GetEventsForStudent(_student);

                if (_viewModel.Events == null || _viewModel.Events.Count <= 0)
                {
                    SetMessage("No event found for today! ", Message.Category.Warning);
                    return View(_viewModel);
                }

                _viewModel.Events.ForEach(e =>
                {
                    e.Status = _eventLogic.GetEventStatus(e);
                });

                _viewModel.Events = _viewModel.Events.OrderBy(e => e.Event_Type_Id).ThenBy(e => e.Event_Start).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }
        public JsonResult MarkAttendance(long eventId, decimal latitude, decimal longitude)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                EVENT currentEvent = _eventLogic.GetEntityBy(e => e.Id == eventId);

                bool eventStatus = _eventLogic.GetEventStatus(currentEvent);
                if (eventStatus)
                {
                    bool inLocation = _eventLogic.InLocation(currentEvent, latitude, longitude);

                    if (inLocation)
                    {
                        _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                        //check if attendance record exist
                        ATTENDANCE attendance = _attendanceLogic.GetEntitiesBy(a => a.Event_Id == eventId && a.Student_Id == _student.Person_Id).LastOrDefault();
                        if (attendance == null)
                            _attendanceLogic.PopulateAttendanceForEvent(currentEvent);

                        _attendanceLogic.MarkAttendance(_student, currentEvent, AttendanceStatuses.Present);

                        result.IsError = false;
                        result.Message = "Attendance Marked";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "You are not currently at the venue of this event.";
                    }
                }
                else
                {
                    result.IsError = true;
                    result.Message = "Event is not ongoing";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveAbsenceRequest(long eventId, int absentType, string dateRange, string remark)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                string[] dateRangeSplit = dateRange.Split('-');
                DateTime dateFrom = DateTime.Now;
                DateTime dateTo = DateTime.Now;

                if (!DateTime.TryParse(dateRangeSplit[0].Trim(), out dateFrom))
                    dateFrom = new DateTime(1900,1,1);
                if (!DateTime.TryParse(dateRangeSplit[1].Trim(), out dateTo))
                    dateTo = new DateTime(1900, 1, 1);

                if (dateFrom == new DateTime(1900, 1, 1) || dateTo == new DateTime(1900, 1, 1) || dateTo < dateFrom)
                {
                    result.IsError = true;
                    result.Message = "Invalid date range";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                ABSENT_LOG absenceLog = _absentLogLogic.GetEntitiesBy(a => a.Event_Id == eventId && a.Student_Id == _student.Person_Id).LastOrDefault();
                if (absenceLog == null)
                {
                    absenceLog = new ABSENT_LOG();
                    absenceLog.Absent_Type_Id = absentType;
                    absenceLog.End_Date = dateTo;
                    absenceLog.Start_Date = dateFrom;
                    absenceLog.Event_Id = eventId;
                    absenceLog.Remark = remark;
                    absenceLog.Student_Id = _student.Person_Id;

                    absenceLog = _absentLogLogic.Create(absenceLog);

                    result.IsError = false;
                    result.Message = "Absence request logged.";
                }
                else
                {
                    absenceLog.Absent_Type_Id = absentType;
                    absenceLog.End_Date = dateTo;
                    absenceLog.Start_Date = dateFrom;
                    absenceLog.Remark = remark;

                    _absentLogLogic.Modify(absenceLog);

                    result.IsError = false;
                    result.Message = "Absence request modified.";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}