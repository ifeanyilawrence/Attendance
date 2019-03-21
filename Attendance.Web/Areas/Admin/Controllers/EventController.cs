using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Admin.Models;
using Attendance.Web.Controllers;
using Attendance.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Attendance.Web.Areas.Admin.Controllers
{
    public class EventController : BaseController
    {
        private EventViewModel _viewModel;
        public ActionResult ViewEventsByDate()
        {
            _viewModel = new EventViewModel();
            try
            {
                EventLogic eventLogic = new EventLogic();

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                _viewModel.Events = eventLogic.GetEntitiesBy(s => s.Date == today);

                _viewModel.Events.ForEach(e =>
                {
                    e.Status = eventLogic.GetEventStatus(e);
                });

                ViewBag.Programmes = _viewModel.ProgrammeSelectList;
                ViewBag.Departments = _viewModel.DepartmentSelectList;
                ViewBag.Levels = _viewModel.LevelSelectList;
                ViewBag.Sessions = _viewModel.SessionSelectList;
                ViewBag.Locations = _viewModel.LocationSelectList;
                ViewBag.Courses = _viewModel.CourseSelectList;
                ViewBag.Halls = _viewModel.HallSelectList;
                ViewBag.EventTypes = _viewModel.EventTypeSelectList;
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(_viewModel);
        }
        [HttpPost]
        public ActionResult ViewEventsByDate(EventViewModel viewModel)
        {
            try
            {
                if (viewModel != null && !string.IsNullOrEmpty(viewModel.DateString))
                {
                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(viewModel.DateString, out date))
                        date = DateTime.Now;

                    EventLogic eventLogic = new EventLogic();

                    viewModel.Events = eventLogic.GetEntitiesBy(s => s.Date == date);
                    viewModel.Events.ForEach(e =>
                    {
                        e.Status = eventLogic.GetEventStatus(e);
                    });
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            ViewBag.Programmes = viewModel.ProgrammeSelectList;
            ViewBag.Departments = viewModel.DepartmentSelectList;
            ViewBag.Levels = viewModel.LevelSelectList;
            ViewBag.Sessions = viewModel.SessionSelectList;
            ViewBag.Locations = viewModel.LocationSelectList;
            ViewBag.Courses = viewModel.CourseSelectList;
            ViewBag.Halls = viewModel.HallSelectList;
            ViewBag.EventTypes = viewModel.EventTypeSelectList;

            return View(viewModel);
        }
        public JsonResult GetEvent(long eventId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (eventId > 0)
                {
                    EventLogic eventLogic = new EventLogic();
                    EVENT myEvent = eventLogic.GetEntityBy(c => c.Id == eventId);

                    if (myEvent != null)
                    {
                        result.EventId = myEvent.Id;
                        result.EventTypeId = myEvent.Event_Type_Id;
                        result.CourseId = myEvent.COURSE != null ? Convert.ToInt64(myEvent.Course_Id) : 0;
                        result.HallId = myEvent.HALL != null ? Convert.ToInt32(myEvent.Hall_Id) : 0;
                        result.EventStart = myEvent.Event_Start.ToShortTimeString();
                        result.EventEnd = myEvent.Event_End.ToShortTimeString();
                        result.LocationId = myEvent.Location_Id;
                        result.Date = myEvent.Date.ToShortDateString();
                        result.SessionId = myEvent.Session_Id;
                        result.ProgrammeId = myEvent.Programme_Id;
                        result.DepartmentId = myEvent.Department_Id;
                        result.LevelId = myEvent.Level_Id;
                        result.EventActive = myEvent.Active != null ? Convert.ToBoolean(myEvent.Active) : false;
                        result.IsWeekly = myEvent.Is_Weekly != null ? Convert.ToBoolean(myEvent.Is_Weekly) : false;
                    }

                    result.IsError = false;
                }
                else
                {
                    result.IsError = true;
                    result.Message = "Invalid parameter";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveEvent(string eventData)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                EventModel eventModel = new JavaScriptSerializer().Deserialize<EventModel>(eventData);
                
                    if (!string.IsNullOrEmpty(eventData) && eventModel != null && !string.IsNullOrEmpty(eventModel.EventId) && !string.IsNullOrEmpty(eventModel.EventStart) && !string.IsNullOrEmpty(eventModel.EventEnd)
                    && !string.IsNullOrEmpty(eventModel.LocationId) && !string.IsNullOrEmpty(eventModel.Date) && !string.IsNullOrEmpty(eventModel.SessionId)
                    && !string.IsNullOrEmpty(eventModel.ProgrammeId) && !string.IsNullOrEmpty(eventModel.DepartmentId) && !string.IsNullOrEmpty(eventModel.LevelId))
                    {
                        EventLogic eventLogic = new EventLogic();
                    UserLogic userLogic = new UserLogic();
                    
                    DateTime eventDate, eventStart, eventEnd;

                    if (!DateTime.TryParse(eventModel.Date, out eventDate))
                        eventDate = DateTime.Now;
                    if (!DateTime.TryParse(eventModel.EventStart, out eventStart))
                        eventStart = DateTime.Now;
                    if (!DateTime.TryParse(eventModel.EventEnd, out eventEnd))
                        eventEnd = DateTime.Now;
                    
                    long courseId = !string.IsNullOrEmpty(eventModel.CourseId) ? Convert.ToInt64(eventModel.CourseId) : 0;
                    int deptId = Convert.ToInt32(eventModel.DepartmentId);
                    int eventTypeId = Convert.ToInt32(eventModel.EventTypeId);
                    int hallId = !string.IsNullOrEmpty(eventModel.HallId) ? Convert.ToInt32(eventModel.HallId) : 0;
                    int levelId = Convert.ToInt32(eventModel.LevelId);
                    int locationId = Convert.ToInt32(eventModel.LocationId);
                    int progId = Convert.ToInt32(eventModel.ProgrammeId);
                    int sessionId = Convert.ToInt32(eventModel.SessionId);
                    long eventId = Convert.ToInt64(eventModel.EventId);
                    
                    EVENT existingEvent = eventLogic.GetEntitiesBy(c => c.Course_Id == courseId && c.Date == eventDate && c.Department_Id == deptId &&
                                            c.Event_End == eventEnd && c.Event_Start == eventStart && c.Event_Type_Id == eventTypeId && c.Hall_Id == hallId &&
                                            c.Level_Id == levelId && c.Location_Id == locationId && c.Programme_Id == progId &&
                                            c.Session_Id == sessionId && c.Id != eventId).LastOrDefault();
                    if (existingEvent == null)
                    {
                        existingEvent = eventLogic.GetEntityBy(c => c.Id == eventId);
                        
                        existingEvent.Active = Convert.ToBoolean(eventModel.EventActive);
                        if(courseId > 0)
                            existingEvent.Course_Id = courseId;
                        existingEvent.Date = eventDate;
                        existingEvent.Department_Id = deptId;
                        existingEvent.Event_End = eventEnd;
                        existingEvent.Event_Start = eventStart;
                        existingEvent.Event_Type_Id = eventTypeId;
                        if(hallId > 0)
                            existingEvent.Hall_Id = hallId;
                        existingEvent.Is_Weekly = Convert.ToBoolean(eventModel.IsWeekly);
                        existingEvent.Level_Id = levelId;
                        existingEvent.Location_Id = locationId;
                        existingEvent.Programme_Id = progId;
                        existingEvent.Session_Id = sessionId;
                        existingEvent.User_Id = userLogic.GetEntitiesBy(u => u.Username == User.Identity.Name).LastOrDefault().Id;

                        eventLogic.Modify(existingEvent);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Event with these parameters already exists.";
                    }
                }
                else
                {
                    result.IsError = true;
                    result.Message = "Invalid parameter";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateEvent(string eventData)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                EventModel eventModel = new JavaScriptSerializer().Deserialize<EventModel>(eventData);

                if (!string.IsNullOrEmpty(eventData) && eventModel != null && !string.IsNullOrEmpty(eventModel.EventStart) && !string.IsNullOrEmpty(eventModel.EventEnd)
                    && !string.IsNullOrEmpty(eventModel.LocationId) && !string.IsNullOrEmpty(eventModel.Date) && !string.IsNullOrEmpty(eventModel.SessionId) 
                    && !string.IsNullOrEmpty(eventModel.ProgrammeId) && !string.IsNullOrEmpty(eventModel.DepartmentId) && !string.IsNullOrEmpty(eventModel.LevelId))
                {
                    EventLogic eventLogic = new EventLogic();
                    UserLogic userLogic = new UserLogic();

                    DateTime eventDate, eventStart, eventEnd;

                    if (!DateTime.TryParse(eventModel.Date, out eventDate))
                        eventDate = DateTime.Now;
                    if (!DateTime.TryParse(eventModel.EventStart, out eventStart))
                        eventStart = DateTime.Now;
                    if (!DateTime.TryParse(eventModel.EventEnd, out eventEnd))
                        eventEnd = DateTime.Now;

                    long courseId = !string.IsNullOrEmpty(eventModel.CourseId) ? Convert.ToInt64(eventModel.CourseId) : 0;
                    int deptId = Convert.ToInt32(eventModel.DepartmentId);
                    int eventTypeId = Convert.ToInt32(eventModel.EventTypeId);
                    int hallId = !string.IsNullOrEmpty(eventModel.HallId) ? Convert.ToInt32(eventModel.HallId) : 0;
                    int levelId = Convert.ToInt32(eventModel.LevelId);
                    int locationId = Convert.ToInt32(eventModel.LocationId);
                    int progId = Convert.ToInt32(eventModel.ProgrammeId);
                    int sessionId = Convert.ToInt32(eventModel.SessionId);

                    EVENT existingEvent = eventLogic.GetEntitiesBy(c => c.Course_Id == courseId && c.Date == eventDate && c.Department_Id == deptId &&
                                            c.Event_End == eventEnd && c.Event_Start == eventStart && c.Event_Type_Id == eventTypeId && c.Hall_Id == hallId &&
                                            c.Level_Id == levelId && c.Location_Id == locationId && c.Programme_Id == progId &&
                                            c.Session_Id == sessionId).LastOrDefault();
                    if (existingEvent == null)
                    {
                        existingEvent = new EVENT();

                        existingEvent.Active = Convert.ToBoolean(eventModel.EventActive);
                        if(courseId > 0)
                            existingEvent.Course_Id = courseId;
                        existingEvent.Date = eventDate;
                        existingEvent.Department_Id = deptId;
                        existingEvent.Event_End = eventEnd;
                        existingEvent.Event_Start = eventStart;
                        existingEvent.Event_Type_Id = eventTypeId;
                        if(hallId > 0)
                            existingEvent.Hall_Id = hallId;
                        existingEvent.Is_Weekly = Convert.ToBoolean(eventModel.IsWeekly);
                        existingEvent.Level_Id = levelId;
                        existingEvent.Location_Id = locationId;
                        existingEvent.Programme_Id = progId;
                        existingEvent.Session_Id = sessionId;
                        existingEvent.User_Id = userLogic.GetEntitiesBy(u => u.Username == User.Identity.Name).LastOrDefault().Id;

                        eventLogic.Create(existingEvent);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Event with these parameters already exist.";
                    }
                }
                else
                {
                    result.IsError = true;
                    result.Message = "Invalid parameter";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteEvent(long eventId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (eventId > 0)
                {
                    AbsentLogLogic logLogic = new AbsentLogLogic();
                    EventLogic eventLogic = new EventLogic();
                    AttendanceLogic attendanceLogic = new AttendanceLogic();

                    ABSENT_LOG eventLog = logLogic.GetEntitiesBy(e => e.Event_Id == eventId).LastOrDefault();
                    ATTENDANCE eventAttendance = attendanceLogic.GetEntitiesBy(e => e.Event_Id == eventId).LastOrDefault();

                    if (eventLog == null && eventAttendance == null)
                    {
                        eventLogic.Delete(c => c.Id == eventId);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Event is already attached to an attendance / absent log";
                    }
                }
                else
                {
                    result.IsError = true;
                    result.Message = "Invalid parameter";
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAttendance()
        {
            _viewModel = new EventViewModel();
            try
            {
                ViewBag.EventTypes = _viewModel.EventTypeSelectList;
                if (User.IsInRole("Lecturer"))
                {
                    _viewModel.EventTypeSelectList = _viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.Lecture) || e.Value == "").ToList();
                    ViewBag.EventTypes = new SelectList(_viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.Lecture);
                }
                    
                if (User.IsInRole("Hall Staff"))
                {
                    _viewModel.EventTypeSelectList = _viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.HallAttendance) || e.Value == "").ToList();
                    ViewBag.EventTypes = new SelectList(_viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.HallAttendance);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(_viewModel);
        }
        [HttpPost]
        public ActionResult GetAttendance(EventViewModel viewModel)
        {
            try
            {
                if (viewModel != null && !string.IsNullOrEmpty(viewModel.DateString) && viewModel.EventType != null && viewModel.EventType.Id > 0)
                {
                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(viewModel.DateString, out date))
                        date = DateTime.Now;

                    EventLogic eventLogic = new EventLogic();
                    viewModel.Events = eventLogic.GetEntitiesBy(s => s.Date == date && s.Event_Type_Id == viewModel.EventType.Id);

                    viewModel.Events.ForEach(e =>
                    {
                        e.Status = eventLogic.GetEventStatus(e);
                    });
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            ViewBag.EventTypes = viewModel.EventTypeSelectList;
            //if (User.IsInRole("Lecturer"))
            //{
            //    viewModel.EventTypeSelectList = viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.Lecture) || e.Value == "").ToList();
            //    ViewBag.EventTypes = new SelectList(viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.Lecture);
            //}

            //if (User.IsInRole("Hall Staff"))
            //{
            //    viewModel.EventTypeSelectList = viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.HallAttendance) || e.Value == "").ToList();
            //    ViewBag.EventTypes = new SelectList(viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.HallAttendance);
            //}

            return View(viewModel);
        }
        public ActionResult ViewAttendance(string eventId)
        {
            _viewModel = new EventViewModel();
            try
            {
                if (!string.IsNullOrEmpty(eventId))
                {
                    long myEventId = Convert.ToInt64(Utility.Decrypt(eventId));

                    AttendanceLogic attendanceLogic = new AttendanceLogic();
                    AbsentLogLogic absentLogLogic = new AbsentLogLogic();
                    
                    _viewModel.AttendanceList = attendanceLogic.GetEntitiesBy(s => s.Event_Id == myEventId);

                    _viewModel.AttendanceList.ForEach(e =>
                    {
                        if (e.Attendance_Status_Id == (int)AttendanceStatuses.Excused)
                            e.ATTENDANCE_STATUS.Name = absentLogLogic.GetAbsenceRequestStatus(e);
                    });

                    _viewModel.EventId = myEventId;
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(_viewModel);
        }
        
        public ActionResult DownloadAttendance(string eventId)
        {
            _viewModel = new EventViewModel();
            try
            {
                if (!string.IsNullOrEmpty(eventId))
                {
                    long myEventId = Convert.ToInt64(Utility.Decrypt(eventId));

                    AttendanceLogic attendanceLogic = new AttendanceLogic();
                    AbsentLogLogic absentLogLogic = new AbsentLogLogic();

                    _viewModel.AttendanceList = attendanceLogic.GetEntitiesBy(s => s.Event_Id == myEventId);

                    _viewModel.AttendanceList.ForEach(e =>
                    {
                        if (e.Attendance_Status_Id == (int)AttendanceStatuses.Excused)
                            e.ATTENDANCE_STATUS.Name = absentLogLogic.GetAbsenceRequestStatus(e);
                    });

                    GridView gv = new GridView();
                    DataTable ds = new DataTable();
                    if (_viewModel.AttendanceList.Count > 0)
                    {
                        List<ATTENDANCE> list = _viewModel.AttendanceList.OrderBy(p => p.STUDENT.Matric_Number).ToList();
                        List<AttendanceModel> sort = new List<AttendanceModel>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            AttendanceModel attendance = new AttendanceModel();
                            attendance.SN = (i + 1);
                            attendance.Name = list[i].STUDENT.PERSON.Last_Name + " " + list[i].STUDENT.PERSON.First_Name + " " + list[i].STUDENT.PERSON.Other_Name;
                            attendance.Registration_Number = list[i].STUDENT.Matric_Number;
                            attendance.Event_Type = list[i].EVENT.EVENT_TYPE.Name;
                            if (list[i].EVENT.COURSE != null)
                                attendance.Course_Hall = list[i].EVENT.COURSE.Name;
                            else if (list[i].EVENT.HALL != null)
                                attendance.Course_Hall = list[i].EVENT.HALL.Name;
                            else
                                attendance.Course_Hall = "";
                            attendance.Location = list[i].EVENT.LOCATION.Name;
                            attendance.Date = list[i].EVENT.Date.ToLongDateString();
                            attendance.Time_Taken = list[i].Time_Taken.ToLongTimeString();
                            attendance.Status = list[i].ATTENDANCE_STATUS.Name;

                            sort.Add(attendance);
                        }

                        gv.DataSource = sort;
                        string caption = "Attendnace Report";
                        if (list != null && list.Count > 0)
                        {
                            caption = "Attendnace Report for " + list.FirstOrDefault().EVENT.DEPARTMENT.Name + ". " + list.FirstOrDefault().EVENT.LEVEL.Name + ". Session: " +
                                            list.FirstOrDefault().EVENT.SESSION.Name;
                        }

                        gv.Caption = caption.ToUpper();
                        gv.DataBind();

                        string filename = caption.Replace("\\", "") + ".xls";
                        return new DownloadFileActionResult(gv, filename);
                    }
                    else
                    {
                        Response.Write("No data available for download");
                        Response.End();
                        return new JavaScriptResult();
                    }
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return RedirectToAction("ViewAttendance", "Event", new { Area = "Admin", eventId = eventId });
        }
        public ActionResult SemesterAttendance()
        {
            _viewModel = new EventViewModel();
            try
            {
                ViewBag.Programmes = _viewModel.ProgrammeSelectList;
                ViewBag.Departments = _viewModel.DepartmentSelectList;
                ViewBag.Levels = _viewModel.LevelSelectList;
                ViewBag.Sessions = _viewModel.SessionSelectList;
                ViewBag.Locations = _viewModel.LocationSelectList;
                ViewBag.Courses = new SelectList(new List<SelectListItem>(), "Value", "Text"); ;
                ViewBag.Halls = _viewModel.HallSelectList;
                ViewBag.EventTypes = _viewModel.EventTypeSelectList;
                if (User.IsInRole("Lecturer"))
                {
                    _viewModel.EventTypeSelectList = _viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.Lecture) || e.Value == "").ToList();
                    ViewBag.EventTypes = new SelectList(_viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.Lecture);
                }

                if (User.IsInRole("Hall Staff"))
                {
                    _viewModel.EventTypeSelectList = _viewModel.EventTypeSelectList.Where(e => e.Value == Convert.ToString((int)EventTypes.HallAttendance) || e.Value == "").ToList();
                    ViewBag.EventTypes = new SelectList(_viewModel.EventTypeSelectList, "Value", "Text", (int)EventTypes.HallAttendance);
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return View(_viewModel);
        }
        [HttpPost]
        public ActionResult SemesterAttendance(EventViewModel viewModel)
        {
            try
            {
                if (viewModel.Session != null && viewModel.Programme != null && viewModel.Department != null && viewModel.Level != null && viewModel.EventType != null)
                {
                    EventLogic eventLogic = new EventLogic();
                    StudentLevelLogic studentLevelLogic = new StudentLevelLogic();
                    List<STUDENT_LEVEL> studentLevels = new List<STUDENT_LEVEL>();

                    studentLevels = studentLevelLogic.GetEntitiesBy(s => s.Department_Id == viewModel.Department.Id && s.Level_Id == viewModel.Level.Id && s.Programme_Id == viewModel.Programme.Id &&
                                                                        s.Session_Id == viewModel.Session.Id);

                    List<ATTENDANCE> masterAttendance = new List<ATTENDANCE>();

                    for (int i = 0; i < studentLevels.Count; i++)
                    {
                        if (viewModel.EventType.Id == (int)EventTypes.Lecture && viewModel.Course != null && viewModel.Course.Id > 0)
                        {
                            ATTENDANCE attendance = ProcessCourseAttendance(viewModel.Course, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Course_Id == viewModel.Course.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                        else if (viewModel.EventType.Id == (int)EventTypes.HallAttendance && viewModel.Hall != null && viewModel.Hall.Id > 0)
                        {
                            ATTENDANCE attendance = ProcessHallAttendance(viewModel.Hall, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Hall_Id == viewModel.Hall.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                        else
                        {
                            ATTENDANCE attendance = ProcessOtherAttendance(viewModel.EventType, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Event_Type_Id == viewModel.EventType.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                    }

                    viewModel.AttendanceList = masterAttendance;
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            TempData["ViewModel"] = viewModel;

            ViewBag.Programmes = viewModel.ProgrammeSelectList;
            ViewBag.Departments = viewModel.DepartmentSelectList;
            ViewBag.Levels = viewModel.LevelSelectList;
            ViewBag.Sessions = viewModel.SessionSelectList;
            ViewBag.Locations = viewModel.LocationSelectList;
            ViewBag.Courses = new SelectList(new List<SelectListItem>(), "Value", "Text"); ;
            ViewBag.Halls = viewModel.HallSelectList;
            ViewBag.EventTypes = viewModel.EventTypeSelectList;
            return View(viewModel);
        }
        public JsonResult GetCourses(int progId, int deptId, int levelId)
        {
            List<COURSE> courses = new List<COURSE>();
            List<SelectListItem> courseSelectList = new List<SelectListItem>();
            try
            {
                if (progId > 0 && deptId > 0 && levelId > 0)
                {
                    CourseLogic courseLogic = new CourseLogic();

                    courses = courseLogic.GetEntitiesBy(c => c.Department_Id == deptId && c.Level_Id == levelId && c.Programme_Id == progId && c.Active);
                    if (User.IsInRole("Lecturer"))
                    {
                        UserLogic userLogic = new UserLogic();
                        USER user = userLogic.GetEntityBy(u => u.Username == User.Identity.Name);
                        StaffLogic staffLogic = new StaffLogic();
                        StaffCourseLogic staffCourseLogic = new StaffCourseLogic();
                        STAFF staff = staffLogic.GetEntitiesBy(s => s.Person_Id == user.Person_Id).LastOrDefault();
                        if (staff != null)
                        {
                            List<STAFF_COURSE> staffCourses = staffCourseLogic.GetEntitiesBy(s => s.Staff_Id == staff.Person_Id);
                            if (staffCourses != null)
                            {
                                courses = staffCourses.Select(s => s.COURSE).ToList();
                            }
                            else
                            {
                                courses = new List<COURSE>();
                            }
                        }
                    }

                    SelectListItem list = new SelectListItem();
                    list.Value = "";
                    list.Text = "-- Select Course --";
                    courseSelectList.Add(list);

                    courses.ForEach(c =>
                    {
                        SelectListItem myList = new SelectListItem();
                        myList.Value = c.Id.ToString();
                        myList.Text = c.Code + " - " + c.Name;
                        courseSelectList.Add(myList);
                    });
                }
            }
            catch (Exception ex)
            {
            }

            return Json(courseSelectList, JsonRequestBehavior.AllowGet);
        }
        private ATTENDANCE ProcessOtherAttendance(EVENT_TYPE eventType, STUDENT student)
        {
            ATTENDANCE attendance = null;
            try
            {
                AttendanceLogic attendanceLogic = new AttendanceLogic();
                EventLogic eventLogic = new EventLogic();

                int numberOfTimesPresent = attendanceLogic.GetEntitiesBy(s => s.EVENT.Event_Type_Id == eventType.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Present).Count();
                int numberOfLectures = eventLogic.GetEntitiesBy(s => s.Event_Type_Id == eventType.Id && (s.Active == true || s.Active == null)).Count();
                int numberOfAbsence = attendanceLogic.GetEntitiesBy(s => s.EVENT.Event_Type_Id == eventType.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Excused).Count();
                int numberOfLecturesHeld = numberOfLectures - numberOfAbsence;

                double eligibilityPercentage = (Convert.ToDouble(numberOfTimesPresent) / Convert.ToDouble(numberOfLecturesHeld)) * 100.0;

                attendance = new ATTENDANCE();
                attendance.Percentage = eligibilityPercentage;

                //result.ApproximateNumberOfLectures = numberOfLecturesHeld;
                //result.EligibilityPercentage = eligibilityPercentage;
                //result.NumberOfAbsent = numberOfAbsence;
                //result.NumberOfPresent = numberOfTimesPresent;
                //result.TotalNumberOfLectures = numberOfLectures;

                if (eligibilityPercentage > 75)
                    attendance.IsEligible = true;
                else
                    attendance.IsEligible = false;
            }
            catch (Exception)
            {
                throw;
            }

            return attendance;
        }

        private ATTENDANCE ProcessCourseAttendance(COURSE course, STUDENT student)
        {
            ATTENDANCE attendance = null;
            try
            {
                AttendanceLogic attendanceLogic = new AttendanceLogic();
                EventLogic eventLogic = new EventLogic();

                int numberOfTimesPresent = attendanceLogic.GetEntitiesBy(s => s.EVENT.Course_Id == course.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Present).Count();
                int numberOfLectures = eventLogic.GetEntitiesBy(s => s.Course_Id == course.Id && (s.Active == true || s.Active == null)).Count();
                int numberOfAbsence = attendanceLogic.GetEntitiesBy(s => s.EVENT.Course_Id == course.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Excused).Count();
                int numberOfLecturesHeld = numberOfLectures - numberOfAbsence;

                double eligibilityPercentage = (Convert.ToDouble(numberOfTimesPresent) / Convert.ToDouble(numberOfLecturesHeld)) * 100.0;

                attendance = new ATTENDANCE();
                attendance.Percentage = eligibilityPercentage;
                
                //result.ApproximateNumberOfLectures = numberOfLecturesHeld;
                //result.EligibilityPercentage = eligibilityPercentage;
                //result.NumberOfAbsent = numberOfAbsence;
                //result.NumberOfPresent = numberOfTimesPresent;
                //result.TotalNumberOfLectures = numberOfLectures;

                if (eligibilityPercentage > 75)
                    attendance.IsEligible = true;
                else
                    attendance.IsEligible = false;
            }
            catch (Exception)
            {
                throw;
            }

            return attendance;
        }

        private ATTENDANCE ProcessHallAttendance(HALL hall, STUDENT student)
        {
            ATTENDANCE attendance = null;
            try
            {
                AttendanceLogic attendanceLogic = new AttendanceLogic();
                EventLogic eventLogic = new EventLogic();

                int numberOfTimesPresent = attendanceLogic.GetEntitiesBy(s => s.EVENT.Hall_Id == hall.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Present).Count();
                int numberOfLectures = eventLogic.GetEntitiesBy(s => s.Hall_Id == hall.Id && (s.Active == true || s.Active == null)).Count();
                int numberOfAbsence = attendanceLogic.GetEntitiesBy(s => s.EVENT.Hall_Id == hall.Id && s.Student_Id == student.Person_Id &&
                                            s.Attendance_Status_Id == (int)AttendanceStatuses.Excused).Count();
                int numberOfLecturesHeld = numberOfLectures - numberOfAbsence;

                double eligibilityPercentage = (Convert.ToDouble(numberOfTimesPresent) / Convert.ToDouble(numberOfLecturesHeld)) * 100.0;

                attendance = new ATTENDANCE();
                attendance.Percentage = eligibilityPercentage;

                //result.ApproximateNumberOfLectures = numberOfLecturesHeld;
                //result.EligibilityPercentage = eligibilityPercentage;
                //result.NumberOfAbsent = numberOfAbsence;
                //result.NumberOfPresent = numberOfTimesPresent;
                //result.TotalNumberOfLectures = numberOfLectures;

                if (eligibilityPercentage > 75)
                    attendance.IsEligible = true;
                else
                    attendance.IsEligible = false;
            }
            catch (Exception)
            {
                throw;
            }

            return attendance;
        }
        public ActionResult DownloadSemesterAttendance(string attendanceId)
        {
            EventViewModel viewModel = (EventViewModel) TempData["ViewModel"];
            if (viewModel == null)
            {
                return null;
            }
            try
            {
                if (!string.IsNullOrEmpty(attendanceId))
                {
                    long myAttendnaceId = Convert.ToInt64(Utility.Decrypt(attendanceId));

                    EventLogic eventLogic = new EventLogic();
                    StudentLevelLogic studentLevelLogic = new StudentLevelLogic();
                    AttendanceLogic attendanceLogic = new AttendanceLogic();

                    //ATTENDANCE myAttendnace = attendanceLogic.GetEntitiesBy(s => s.Event_Id == myAttendnaceId).LastOrDefault();

                    //if (myAttendnace == null)
                    //{
                    //    return null;
                    //}

                    List<STUDENT_LEVEL> studentLevels = new List<STUDENT_LEVEL>();

                    studentLevels = studentLevelLogic.GetEntitiesBy(s => s.Department_Id == viewModel.Department.Id && s.Level_Id == viewModel.Level.Id && s.Programme_Id == viewModel.Programme.Id &&
                                                                        s.Session_Id == viewModel.Session.Id);

                    List<ATTENDANCE> masterAttendance = new List<ATTENDANCE>();

                    for (int i = 0; i < studentLevels.Count; i++)
                    {
                        if (viewModel.EventType.Id == (int)EventTypes.Lecture && viewModel.Course!= null && viewModel.Course.Id > 0)
                        {
                            ATTENDANCE attendance = ProcessCourseAttendance(viewModel.Course, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Course_Id == viewModel.Course.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                        else if (viewModel.EventType.Id == (int)EventTypes.HallAttendance && viewModel.Hall != null && viewModel.Hall.Id > 0)
                        {
                            ATTENDANCE attendance = ProcessHallAttendance(viewModel.Hall, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Hall_Id == viewModel.Hall.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                        else
                        {
                            ATTENDANCE attendance = ProcessOtherAttendance(viewModel.EventType, studentLevels[i].STUDENT);
                            if (attendance != null)
                            {
                                attendance.STUDENT = studentLevels[i].STUDENT;
                                attendance.EVENT = eventLogic.GetEntitiesBy(e => e.Event_Type_Id == viewModel.EventType.Id).LastOrDefault();
                                masterAttendance.Add(attendance);
                            }
                        }
                    }

                    GridView gv = new GridView();
                    DataTable ds = new DataTable();
                    if (masterAttendance.Count > 0)
                    {
                        List<ATTENDANCE> list = masterAttendance.OrderBy(p => p.STUDENT.Matric_Number).ToList();
                        List<SemesterAttendanceModel> sort = new List<SemesterAttendanceModel>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            SemesterAttendanceModel attendance = new SemesterAttendanceModel();
                            attendance.SN = (i + 1);
                            attendance.Name = list[i].STUDENT.PERSON.Last_Name + " " + list[i].STUDENT.PERSON.First_Name + " " + list[i].STUDENT.PERSON.Other_Name;
                            attendance.Registration_Number = list[i].STUDENT.Matric_Number;
                            attendance.Event_Type = list[i].EVENT.EVENT_TYPE.Name;
                            if (list[i].EVENT.COURSE != null)
                                attendance.Course_Hall = list[i].EVENT.COURSE.Name;
                            else if (list[i].EVENT.HALL != null)
                                attendance.Course_Hall = list[i].EVENT.HALL.Name;
                            else
                                attendance.Course_Hall = "";
                            attendance.Percentage = list[i].Percentage.ToString() + "%";
                            attendance.Status = list[i].IsEligible ? "Eligible" : "Not Eligible";

                            sort.Add(attendance);
                        }

                        gv.DataSource = sort;
                        string caption = "Attendnace Report";
                        if (list != null && list.Count > 0)
                        {
                            caption = "Attendnace Report for " + list.FirstOrDefault().EVENT.DEPARTMENT.Name + ". " + list.FirstOrDefault().EVENT.LEVEL.Name + ". Session: " +
                                            list.FirstOrDefault().EVENT.SESSION.Name;
                        }

                        gv.Caption = caption.ToUpper();
                        gv.DataBind();

                        string filename = caption.Replace("\\", "") + ".xls";
                        return new DownloadFileActionResult(gv, filename);
                    }
                    else
                    {
                        Response.Write("No data available for download");
                        Response.End();
                        return new JavaScriptResult();
                    }
                }
            }
            catch (Exception ex)
            {
                SetMessage("Error! " + ex.Message, Message.Category.Error);
            }

            return RedirectToAction("SemesterAttendance", "Event", new { Area = "Admin"});
        }
    }
}