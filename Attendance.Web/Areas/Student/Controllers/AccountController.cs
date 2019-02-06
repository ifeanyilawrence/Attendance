using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Student.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace Attendance.Web.Areas.Student.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Signup()
        {
            AccountViewModel viewModel = new AccountViewModel();

            //ViewBag.Programme = viewModel.ProgrammeSelectList;
            ViewBag.Department = viewModel.DepartmentSelectList;
            ViewBag.Gender = viewModel.GenderSelectList;
            ViewBag.Level = viewModel.LevelSelectList;
            ViewBag.Hall = viewModel.HallSelectList;

            return View();
        }
        [AllowAnonymous]
        public JsonResult RegisterStudent(string[] studentData)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if(studentData == null)
                {
                    result.IsError = true;
                    result.Message = "Invalid data.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                PersonLogic personLogic = new PersonLogic();
                StudentLogic studentLogic = new StudentLogic();
                StudentLevelLogic studentLevelLogic = new StudentLevelLogic();
                UserLogic userLogic = new UserLogic();

                using (TransactionScope scope = new TransactionScope())
                {
                    PERSON person = new PERSON();
                    person.Last_Name = studentData[0].Trim();
                    person.First_Name = studentData[1].Trim();
                    person.Other_Name = !string.IsNullOrEmpty(studentData[2]) ? studentData[2].Trim() : null;
                    person.Email = !string.IsNullOrEmpty(studentData[3]) ? studentData[3].Trim() : null;
                    person.Phone_Number = !string.IsNullOrEmpty(studentData[4]) ? studentData[4].Trim() : null;
                    person.Gender_Id = Convert.ToInt32(studentData[10]);

                    PERSON createdPerson = personLogic.Create(person);

                    STUDENT student = new STUDENT();
                    student.Active = true;
                    student.Matric_Number = studentData[5].Trim();
                    student.Hall_Id = Convert.ToInt32(studentData[9]);
                    student.Person_Id = createdPerson.Id;

                    STUDENT existingStudent = studentLogic.GetEntityBy(s => s.Matric_Number == student.Matric_Number);
                    if (existingStudent != null)
                    {
                        result.IsError = true;
                        result.Message = "Student with this matric number already exist.";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    studentLogic.Create(student);

                    STUDENT_LEVEL studentLevel = new STUDENT_LEVEL();
                    studentLevel.Department_Id = Convert.ToInt32(studentData[7]);
                    studentLevel.Level_Id = Convert.ToInt32(studentData[8]);
                    studentLevel.Programme_Id = (int)Programmes.Undergraduate;
                    studentLevel.Session_Id = (int)Sessions._2018_2019;
                    studentLevel.Student_Id = createdPerson.Id;

                    studentLevelLogic.Create(studentLevel);

                    USER user = new USER();
                    user.Active = true;
                    user.Password = studentData[6].Trim();
                    user.Person_Id = createdPerson.Id;
                    user.Role_Id = (int)Roles.Student;
                    user.Username = studentData[5].Trim();

                    USER existingUser = userLogic.GetEntityBy(u => u.Username == student.Matric_Number);
                    if (existingUser != null)
                    {
                        result.IsError = true;
                        result.Message = "User with this username already exist.";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    userLogic.Create(user);

                    result.IsError = false;

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}