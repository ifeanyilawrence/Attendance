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
            AccountViewModel viewModel = new AccountViewModel();
            
            ViewBag.Gender = viewModel.GenderSelectList;
            ViewBag.Role = viewModel.RoleSelectList;

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
                    

                    USER user = new USER();
                    user.Active = true;
                    user.Password = signupJsonModel.password.Trim();
                    user.Person_Id = createdPerson.Id;
                    user.Role_Id = Convert.ToInt32(signupJsonModel.role);
                    user.Username = signupJsonModel.userName.Trim();

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