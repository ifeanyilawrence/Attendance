using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Admin.Models;
using Attendance.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance.Web.Areas.Admin.Controllers
{
    public class SetupController : BaseController
    {
        private SetupViewModel _viewModel;
        private CourseLogic _courseLogic;
        private LocationLogic _locationLogic;
        private HallLogic _hallLogic;
        private StaffCourseLogic _staffCourseLogic;
        private StaffHallLogic _staffHallLogic;
        public ActionResult ManageCourse()
        {
            _viewModel = new SetupViewModel();
            _courseLogic = new CourseLogic();
            try
            {
                ViewBag.Programmes = _viewModel.ProgrammeSelectList;
                ViewBag.Departments = _viewModel.DepartmentSelectList;
                ViewBag.Levels = _viewModel.LevelSelectList;

                _viewModel.CourseList = _courseLogic.GetEntitiesBy(c => c.Active);
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }
        public JsonResult GetCourse(long courseId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (courseId > 0)
                {
                    CourseLogic courseLogic = new CourseLogic();
                    COURSE course = courseLogic.GetEntityBy(c => c.Id == courseId);

                    if (course != null)
                    {
                        result.CourseId = course.Id;
                        result.CourseCode = course.Code;
                        result.CourseName = course.Name;
                        result.ProgrammeId = course.Programme_Id;
                        result.DepartmentId = course.Department_Id;
                        result.LevelId = course.Level_Id;
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
        public JsonResult SaveCourse(long courseId, string code, string name, int progId, int deptId, int levelId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (courseId > 0 && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name) && progId > 0 && deptId > 0 && levelId > 0)
                {
                    CourseLogic courseLogic = new CourseLogic();
                    COURSE course = courseLogic.GetEntityBy(c => c.Id == courseId);

                    COURSE existingCourse = courseLogic.GetEntitiesBy(c => c.Code.ToLower().Trim() == code.ToLower().Trim() && c.Id != courseId).LastOrDefault();
                    if (existingCourse == null)
                    {
                        if (course != null)
                        {
                            course.Department_Id = deptId;
                            course.Programme_Id = progId;
                            course.Level_Id = levelId;
                            course.Code = code;
                            course.Name = name;

                            courseLogic.Modify(course);
                        }

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Course with the same code exists.";
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
        public JsonResult CreateCourse(string code, string name, int progId, int deptId, int levelId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name) && progId > 0 && deptId > 0 && levelId > 0)
                {
                    CourseLogic courseLogic = new CourseLogic();
                    COURSE existingCourse = courseLogic.GetEntitiesBy(c => c.Code.ToLower().Trim() == code.ToLower().Trim()).LastOrDefault();
                    if (existingCourse == null)
                    {
                        existingCourse = new COURSE();

                        existingCourse.Department_Id = deptId;
                        existingCourse.Programme_Id = progId;
                        existingCourse.Level_Id = levelId;
                        existingCourse.Code = code;
                        existingCourse.Name = name;
                        existingCourse.Active = true;

                        courseLogic.Create(existingCourse);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Course with the same code exists.";
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
        public JsonResult DeleteCourse(long courseId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (courseId > 0)
                {
                    CourseLogic courseLogic = new CourseLogic();
                    EventLogic eventLogic = new EventLogic();
                    EVENT courseEvent = eventLogic.GetEntitiesBy(e => e.Course_Id == courseId).LastOrDefault();

                    if (courseEvent == null)
                    {
                        courseLogic.Delete(c => c.Id == courseId);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Course is already attached to an event, hence cannot be deleted";
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

        public ActionResult ManageLocation()
        {
            _viewModel = new SetupViewModel();
            _locationLogic = new LocationLogic();
            try
            {
                _viewModel.LocationList = _locationLogic.GetEntitiesBy(c => c.Active);
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }
        public JsonResult GetLocation(int locationId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (locationId > 0)
                {
                    _locationLogic = new LocationLogic();
                    LOCATION location = _locationLogic.GetEntityBy(c => c.Id == locationId);

                    if (location != null)
                    {
                        result.LocationId = location.Id;
                        result.LocationName = location.Name;
                        result.LocationDescription = location.Description;
                        result.Longitude = location.Longitude;
                        result.Latitude = location.Latitude;
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
        public JsonResult SaveLocation(int locationId, string name, string description, decimal longitude, decimal latitude)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (locationId > 0 && !string.IsNullOrEmpty(name) && longitude > 0M && latitude > 0M)
                {
                    _locationLogic = new LocationLogic();
                    LOCATION location = _locationLogic.GetEntityBy(c => c.Id == locationId);

                    LOCATION existingLocation = _locationLogic.GetEntitiesBy(c => c.Latitude == latitude && c.Longitude == longitude && c.Id != locationId).LastOrDefault();
                    if (existingLocation == null)
                    {
                        if (location != null)
                        {
                            location.Name = name;
                            location.Description = description;
                            location.Longitude = longitude;
                            location.Latitude = latitude;

                            _locationLogic.Modify(location);
                        }

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Location with the same cordinates exists.";
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
        public JsonResult CreateLocation(string name, string description, decimal longitude, decimal latitude)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(name) && longitude > 0M && latitude > 0M)
                {
                    _locationLogic = new LocationLogic();
                    LOCATION existingLocation = _locationLogic.GetEntitiesBy(c => c.Latitude == latitude && c.Longitude == longitude).LastOrDefault();
                    if (existingLocation == null)
                    {
                        existingLocation = new LOCATION();

                        existingLocation.Name = name;
                        existingLocation.Description = description;
                        existingLocation.Longitude = longitude;
                        existingLocation.Latitude = latitude;
                        existingLocation.Active = true;

                        _locationLogic.Create(existingLocation);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Location with the same cordinates exists.";
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
        public JsonResult DeleteLocation(int locationId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (locationId > 0)
                {
                    _locationLogic = new LocationLogic();
                    EventLogic eventLogic = new EventLogic();
                    EVENT locationEvent = eventLogic.GetEntitiesBy(e => e.Location_Id == locationId).LastOrDefault();

                    if (locationEvent == null)
                    {
                        _locationLogic.Delete(c => c.Id == locationId);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Location is already attached to an event, hence cannot be deleted";
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

        public ActionResult ManageHall()
        {
            _viewModel = new SetupViewModel();
            _hallLogic = new HallLogic();
            try
            {
                _viewModel.HallList = _hallLogic.GetEntitiesBy(c => c.Active);
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }
        public JsonResult GetHall(int hallId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (hallId > 0)
                {
                    _hallLogic = new HallLogic();
                    HALL hall = _hallLogic.GetEntityBy(c => c.Id == hallId);

                    if (hall != null)
                    {
                        result.HallId = hall.Id;
                        result.HallName = hall.Name;
                        result.HallDescription = hall.Description;
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
        public JsonResult SaveHall(int hallId, string name, string description)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (hallId > 0 && !string.IsNullOrEmpty(name))
                {
                    _hallLogic = new HallLogic();
                    HALL hall = _hallLogic.GetEntityBy(c => c.Id == hallId);

                    HALL existingHall = _hallLogic.GetEntitiesBy(c => c.Name.ToLower().Trim() == name.ToLower().Trim() && c.Id != hallId).LastOrDefault();
                    if (existingHall == null)
                    {
                        if (hall != null)
                        {
                            hall.Name = name;
                            hall.Description = description;

                            _hallLogic.Modify(hall);
                        }

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Hall with the same name exists.";
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
        public JsonResult CreateHall(string name, string description)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    _hallLogic = new HallLogic();
                    HALL existingHall = _hallLogic.GetEntitiesBy(c => c.Name.ToLower().Trim() == name.ToLower().Trim()).LastOrDefault();
                    if (existingHall == null)
                    {
                        HALL maxHall = _hallLogic.GetAll().LastOrDefault();
                        int maxHallId = 0;
                        if (maxHall != null)
                        {
                            maxHallId = maxHall.Id + 1;
                        }
                        else
                        {
                            maxHallId = 1;
                        }

                        existingHall = new HALL();

                        existingHall.Id = maxHallId;
                        existingHall.Name = name;
                        existingHall.Description = description;
                        existingHall.Active = true;

                        _hallLogic.Create(existingHall);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Hall with the same name exists.";
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
        public JsonResult DeleteHall(int hallId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (hallId > 0)
                {
                    _hallLogic = new HallLogic();
                    EventLogic eventLogic = new EventLogic();
                    EVENT hallEvent = eventLogic.GetEntitiesBy(e => e.Hall_Id == hallId).LastOrDefault();

                    if (hallEvent == null)
                    {
                        _hallLogic.Delete(c => c.Id == hallId);

                        result.IsError = false;
                        result.Message = "Operation Successful!";
                    }
                    else
                    {
                        result.IsError = true;
                        result.Message = "Hall is already attached to an event, hence cannot be deleted";
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

        public ActionResult CourseAllocation()
        {
            _viewModel = new SetupViewModel();
            _staffCourseLogic = new StaffCourseLogic();
            try
            {
                ViewBag.Courses = _viewModel.CourseSelectList;
                ViewBag.Staff = _viewModel.LecturerSelectList;

                _viewModel.StaffCourseList = _staffCourseLogic.GetAll();
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }
        public ActionResult HallAllocation()
        {
            _viewModel = new SetupViewModel();
            _staffHallLogic = new StaffHallLogic();
            try
            {
                ViewBag.Halls = _viewModel.HallSelectList;
                ViewBag.Staff = _viewModel.HallStaffSelectList;

                _viewModel.StaffHallList = _staffHallLogic.GetAll();
            }
            catch (Exception)
            {
                throw;
            }

            return View(_viewModel);
        }

        public JsonResult AllocateCourse(long courseId, long staffId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (courseId > 0 && staffId > 0)
                {
                    _staffCourseLogic = new StaffCourseLogic();
                    STAFF_COURSE existingStaffCourse = _staffCourseLogic.GetEntitiesBy(c => c.Course_Id == courseId).LastOrDefault();
                    if (existingStaffCourse == null)
                    {
                        existingStaffCourse = new STAFF_COURSE();

                        existingStaffCourse.Course_Id = courseId;
                        existingStaffCourse.Staff_Id = staffId;

                        _staffCourseLogic.Create(existingStaffCourse);
                    }
                    else
                    {
                        existingStaffCourse.Staff_Id = staffId;

                        _staffCourseLogic.Modify(existingStaffCourse);
                    }

                    result.IsError = false;
                    result.Message = "Operation Successful!";
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
        public JsonResult AllocateHall(int hallId, long staffId)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (hallId > 0 && staffId > 0)
                {
                    _staffHallLogic = new StaffHallLogic();
                    STAFF_HALL existingStaffHall = _staffHallLogic.GetEntitiesBy(c => c.Hall_Id == hallId).LastOrDefault();
                    if (existingStaffHall == null)
                    {
                        existingStaffHall = new STAFF_HALL();

                        existingStaffHall.Hall_Id = hallId;
                        existingStaffHall.Staff_Id = staffId;

                        _staffHallLogic.Create(existingStaffHall);
                    }
                    else
                    {
                        existingStaffHall.Staff_Id = staffId;

                        _staffHallLogic.Modify(existingStaffHall);
                    }

                    result.IsError = false;
                    result.Message = "Operation Successful!";
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
    }
}