using Attendance.Business;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Attendance.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Attendance.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account

        public ActionResult Signup()
        {
            try
            {
                AccountViewModel viewModel = new AccountViewModel();

                string[] rolesToSkip = { "1", "6" };

                ViewBag.Gender = viewModel.GenderSelectList;
                ViewBag.Role = viewModel.RoleSelectList.Where(r => !rolesToSkip.Contains(r.Value)).ToList();
                ViewBag.StaffType = viewModel.StaffTypeSelectList;
            }
            catch (Exception ex)
            {

                throw;
            }

            return View();
        }
        [AllowAnonymous]
        public JsonResult CreateUser(string userData)
        {
            JsonResponseModel result = new JsonResponseModel();
            try
            {
                if (userData == null)
                {
                    result.IsError = true;
                    result.Message = "Invalid data.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


                SignupJsonModel signupJsonModel = new JavaScriptSerializer().Deserialize <SignupJsonModel>(userData);

                PersonLogic personLogic = new PersonLogic();
                UserLogic userLogic = new UserLogic();

                using (TransactionScope scope = new TransactionScope())
                {
                    PERSON person = new PERSON();
                    person.Last_Name = signupJsonModel.surname.Trim();
                    person.First_Name = signupJsonModel.firstname.Trim();
                    person.Other_Name = !string.IsNullOrEmpty(signupJsonModel.othernames) ? signupJsonModel.othernames.Trim() : null;
                    person.Email = !string.IsNullOrEmpty(signupJsonModel.email) ? signupJsonModel.email.Trim() : null;
                    person.Phone_Number = !string.IsNullOrEmpty(signupJsonModel.phoneNumber) ? signupJsonModel.phoneNumber.Trim() : null;
                    person.Gender_Id = Convert.ToInt32(signupJsonModel.gender);

                    PERSON createdPerson = personLogic.Create(person);

                    STAFF staff = new STAFF();
                    staff.PERSON = createdPerson;
                    staff.Registration_Number = !string.IsNullOrEmpty(signupJsonModel.regnumber) ? signupJsonModel.regnumber.Trim() : null;
                    staff.Active = true;
                    staff.Is_Hall_Officer = Convert.ToInt32(signupJsonModel.role) == (int)Roles.HallStaff;
                    staff.Is_Lecturer = Convert.ToInt32(signupJsonModel.role) == (int)Roles.Lecturer;
                    staff.Is_Medical_Staff = Convert.ToInt32(signupJsonModel.role) == (int)Roles.MedicalSTaff;
                    staff.Is_Student_Affairs = Convert.ToInt32(signupJsonModel.role) == (int)Roles.StudentAffairs;
                    
                    USER user = new USER();
                    user.Active = true;
                    user.Password = signupJsonModel.password.Trim();
                    user.Person_Id = createdPerson.Id;
                    user.Role_Id = Convert.ToInt32(signupJsonModel.role);
                    user.Username = !string.IsNullOrEmpty(signupJsonModel.userName) ? signupJsonModel.userName.Trim() : signupJsonModel.regnumber.Trim();

                    USER existingUser = userLogic.GetEntityBy(u => u.Username == signupJsonModel.userName);
                    if (existingUser != null)
                    {
                        result.IsError = true;
                        result.Message = "User with this username already exist.";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    userLogic.Create(user);

                    result.IsError = false;
                    result.Message = "Operation Sucessful";

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