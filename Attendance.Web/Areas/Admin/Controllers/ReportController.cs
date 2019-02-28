using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Models;
using Newtonsoft.Json;

namespace Attendance.Web.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Admin/Report
        public ActionResult AbsenceRequestReport()
        {

            ViewBag.Course = Utility.PopulateCourseSelectListItem();
            ViewBag.Level = Utility.PopulateLevelSelectListItem();

            return View();
        }

        public JsonResult GetCourses(string level)
        {
            try
            {
                if (string.IsNullOrEmpty(level))
                {
                    return null;
                }
                LEVEL levelentity = new LEVEL { Id = Convert.ToInt32(level) };
                List<COURSE> courseSelectList = new List<COURSE>();
                CourseLogic courseLogic = new CourseLogic();
                courseSelectList = courseLogic.GetEntitiesBy(p => p.Level_Id == levelentity.Id);

                return Json(new SelectList(courseSelectList, Utility.ID,Utility. NAME), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public JsonResult GetAbsentRecord(string level ,string course)
        {
            try
            {
                if (string.IsNullOrEmpty(level) && string.IsNullOrEmpty(course))
                {
                    return null;
                }
                AbsentLogLogic absentLogLogic = new AbsentLogLogic();
                List<AbsentLogModel> absentLogs = new List<AbsentLogModel>();

                LEVEL levelentity = new LEVEL { Id = Convert.ToInt32(level) };
                COURSE courseentity = new COURSE() { Id = Convert.ToInt32(course) };

                absentLogs = absentLogLogic.GetBy(levelentity, courseentity);

                return Json(absentLogs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return null;
        }

        public JsonResult UpdateAbsentlog(string updateArray)
        {
            JsonResponseModel model = new JsonResponseModel();
            try
            {
                int modifiedCount = 0;
                if (string.IsNullOrEmpty(updateArray))
                {
                    return null;
                }
                List<AbsentLogModel> absentLogModels = JsonConvert.DeserializeObject<List<AbsentLogModel>>(updateArray);
                AbsentLogLogic absentLogLogic = new AbsentLogLogic();

                for (int i = 0; i < absentLogModels.Count; i++)
                {
                    long id = absentLogModels[i].Id;
                    ABSENT_LOG absentLog = absentLogLogic.GetEntityBy(a => a.Id == id);

                    if (absentLog != null)
                    {
                        if (absentLogModels[i].Accept)
                        {
                            absentLog.Approved = absentLogModels[i].Accept;
                        }
                        if (absentLogModels[i].Decline)
                        {
                            absentLog.Approved = !absentLogModels[i].Decline;
                            absentLog.Reject_Reason = absentLogModels[i].RejectReason;
                            absentLog.Remark = absentLogModels[i].Remark;
                        }

                        var modified = absentLogLogic.Modify(absentLog);
                        if (modified)
                        {
                            modifiedCount += 1;
                        }
                    } 
                }
                if (modifiedCount > 0)
                {
                    model.IsError = false;
                    model.Message = "Operation Successful";
                }
                else
                {
                    model.IsError = true;
                    model.Message = "Nothing found to be Updated";
                }
            }
            catch (Exception ex)
            {
                model.IsError = true;
                model.Message = ex.Message +  "Operation Failed";
                
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}