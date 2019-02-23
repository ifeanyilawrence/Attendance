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
        private CourseLogic _courseLogic;
        public AttendanceController()
        {
            _studentLogic = new StudentLogic();
            _eventLogic = new EventLogic();
            _absentTypeLogic = new AbsentTypeLogic();
            _attendanceLogic = new AttendanceLogic();
            _absentLogLogic = new AbsentLogLogic();
            _courseLogic = new CourseLogic();
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
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
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
        public ActionResult ViewMyAttendance()
        {
            _viewModel = new AttendanceViewModel();
            return View(_viewModel);
        }
        [HttpPost]
        public ActionResult ViewMyAttendance(AttendanceViewModel viewModel)
        {
            try
            {
                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                DateTime date = new DateTime();
                if (!DateTime.TryParse(viewModel.Date, out date))
                    date = DateTime.Now;

                viewModel.AttendanceList = _attendanceLogic.GetAttendanceForStudent(_student, date);

                if (viewModel.AttendanceList == null || viewModel.AttendanceList.Count <= 0)
                {
                    SetMessage("No attendance record found for today! ", Message.Category.Warning);
                    return View(viewModel);
                }

                viewModel.AttendanceList = viewModel.AttendanceList.OrderBy(s => s.EVENT.Date).ThenBy(s => s.EVENT.Event_Type_Id).ToList();
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(viewModel);
        }
        public ActionResult ViewAbsenceRequest()
        {
            _viewModel = new AttendanceViewModel();
            try
            {
                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                _viewModel.AbsenceList = _absentLogLogic.GetEntitiesBy(a => a.Student_Id == _student.Person_Id);

                if (_viewModel.AbsenceList == null || _viewModel.AbsenceList.Count <= 0)
                {
                    SetMessage("No absence requests found for! ", Message.Category.Warning);
                    return View(_viewModel);
                }

                _viewModel.AbsenceList = _viewModel.AbsenceList.OrderBy(s => s.EVENT.Date).ThenBy(s => s.EVENT.Event_Type_Id).ToList();
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(_viewModel);
        }
        public ActionResult CheckExamEligibility()
        {
            try
            {
                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                ViewBag.Courses = _courseLogic.GetCoursesForStudent(_student);
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }
            return View();
        }
        public JsonResult GetCourseEligibility(long courseId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                _student = _studentLogic.GetEntityBy(s => s.Matric_Number == User.Identity.Name);

                int numberOfTimesPresent = _attendanceLogic.GetEntitiesBy(s => s.EVENT.Course_Id == courseId && s.Student_Id == _student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Present).Count();
                int numberOfLectures = _eventLogic.GetEntitiesBy(s => s.Course_Id == courseId).Count();
                int numberOfAbsence = _attendanceLogic.GetEntitiesBy(s => s.EVENT.Course_Id == courseId && s.Student_Id == _student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Excused).Count();
                int numberOfLecturesHeld = numberOfLectures - numberOfAbsence;

                double eligibilityPercentage = (numberOfTimesPresent / numberOfLecturesHeld) * 100;

                result.ApproximateNumberOfLectures = numberOfLecturesHeld;
                result.EligibilityPercentage = eligibilityPercentage;
                result.NumberOfAbsent = numberOfAbsence;
                result.NumberOfPresent = numberOfTimesPresent;
                result.TotalNumberOfLectures = numberOfLectures;

                if (eligibilityPercentage > 75)
                {
                    result.IsEligible = true;
                    result.Message = "Congratulations! You are eligible.";
                }
                else
                {
                    result.IsEligible = false;
                    result.Message = "Sorry! You are not eligible.";
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