using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Linq.Expressions;
using Attendance.Business;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Attendance.Model.Entity;
using Attendance.Model.Model;
using Microsoft.Ajax.Utilities;

namespace Attendance.Web.Models
{
    public class Utility
    {
        public const string ID = "Id";
        public const string NAME = "Name";
        public const string VALUE = "Value";
        public const string TEXT = "Text";
        public const string Select = "-- Select --";
        public const string DEFAULT_AVATAR = "/Content/Images/default_avatar.png";
        public const string SelectDepartment = "-- Select Department --";
        public const string SelectSession = "-- Select Session --";
        public const string SelectLevel = "-- Select Level --";
        public const string SelectProgramme = "-- Select Programme --";
        public const string SelectCourse = "-- Select Course --";
        
        public static void BindDropdownItem<T>(DropDownList dropDownList, T items, string dataValueField, string dataTextField)
        {
            dropDownList.Items.Clear();

            dropDownList.DataValueField = dataValueField;
            dropDownList.DataTextField = dataTextField;
           

            dropDownList.DataSource = items;
            dropDownList.DataBind();
        }

        public static List<Value> CreateYearListFrom(int startYear)
        {
            List<Value> years = new List<Value>();

            try
            {
                DateTime currentDate = DateTime.Now;
                int currentYear = currentDate.Year;

                for (int i = startYear; i <= currentYear; i++)
                {
                    Value value = new Value();
                    value.Id = i;
                    value.Name = i.ToString();
                    years.Add(value);
                }

                //years.Insert(0, new Value() { Id = 0, Name = Select });
                return years;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<Value> CreateYearListFrom(int startYear, int endYear)
        {
            List<Value> years = new List<Value>();

            try
            {



                for (int i = startYear; i <= endYear; i++)
                {
                    Value value = new Value();
                    value.Id = i;
                    value.Name = i.ToString();
                    years.Add(value);
                }

                //years.Insert(0, new Value() { Id = 0, Name = Select });
                return years;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Value> CreateNumberListFrom(int startValue, int endValue)
        {
            List<Value> values = new List<Value>();

            try
            {
                for (int i = startValue; i <= endValue; i++)
                {
                    Value value = new Value();
                    value.Id = i;
                    value.Name = i.ToString();
                    values.Add(value);
                }

                //values.Insert(0, new Value() { Id = 0, Name = Select });
                return values;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Value> GetMonthsInYear()
        {
            List<Value> values = new List<Value>();

            try
            {
                string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;

                for (int i = 0; i < names.Length; i++)
                {
                    int j = i + 1;
                    Value value = new Value();
                    value.Id = j;
                    value.Name = names[i];
                    values.Add(value);
                }

                //values.Insert(0, new Value() { Id = 0, Name = Select });
                return values;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateGraduationMonthSelectListItem()
        {
            try
            {
                List<Value> months = GetMonthsInYear();
                if (months == null || months.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> monthList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                monthList.Add(list);

                foreach (Value month in months)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = month.Id.ToString();
                    selectList.Text = month.Name;

                    monthList.Add(selectList);
                }

                return monthList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        public static List<SelectListItem> PopulateYearSelectListItem(int startYear, bool withSelect)
        {
            try
            {
                int END_YEAR = DateTime.Now.Year + 2;
                List<Value> years = CreateYearListFrom(startYear, END_YEAR);
                if (years == null || years.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> yearList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                if (withSelect)
                {
                    list.Text = Select;
                }
                else
                {
                    list.Text = "--YY--";
                }

                yearList.Add(list);

                foreach (Value year in years)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = year.Id.ToString();
                    selectList.Text = year.Name;

                    yearList.Add(selectList);
                }

                return yearList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateGenderSelectListItem()
        {
            try
            {
                GenderLogic genderLogic = new GenderLogic();
                List<GENDER> genders = genderLogic.GetEntitiesBy(p => p.Active);
                if (genders == null || genders.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> genderList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                genderList.Add(list);

                foreach (GENDER gender in genders)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = gender.Id.ToString();
                    selectList.Text = gender.Name;

                    genderList.Add(selectList);
                }

                return genderList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateAllDepartmentSelectListItem()
        {
            try
            {
                DepartmentLogic departmentLogic = new DepartmentLogic();
                List<DEPARTMENT> departments = departmentLogic.GetEntitiesBy(p => p.Active);

                if (departments == null || departments.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> deptList = new List<SelectListItem>();
                if (departments != null && departments.Count > 0)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = "";
                    list.Text = Select;
                    deptList.Add(list);

                    foreach (DEPARTMENT department in departments)
                    {
                        SelectListItem selectList = new SelectListItem();
                        selectList.Value = department.Id.ToString();
                        selectList.Text = department.Name;
                        deptList.Add(selectList);
                    }
                }

                return deptList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateLevelSelectListItem()
        {
            try
            {
                LevelLogic levelLogic = new LevelLogic();
                List<LEVEL> levels = levelLogic.GetEntitiesBy(p => p.Active);

                if (levels == null || levels.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> levelList = new List<SelectListItem>();
                if (levels != null && levels.Count > 0)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = "";
                    list.Text = Select;
                    levelList.Add(list);

                    foreach (LEVEL level in levels)
                    {
                        SelectListItem selectList = new SelectListItem();
                        selectList.Value = level.Id.ToString();
                        selectList.Text = level.Name;
                        levelList.Add(selectList);
                    }
                }

                return levelList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateAllProgrammeSelectListItem()
        {
            try
            {
                ProgrammeLogic programmeLogic = new ProgrammeLogic();
                List<PROGRAMME> programmes = programmeLogic.GetEntitiesBy(p => p.Active);

                if (programmes == null || programmes.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> selectItemList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = SelectProgramme;
                selectItemList.Add(list);

                foreach (PROGRAMME programme in programmes)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = programme.Id.ToString();
                    selectList.Text = programme.Name;

                    selectItemList.Add(selectList);
                }

                return selectItemList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateHallSelectListItem()
        {
            try
            {
                HallLogic hallLogic = new HallLogic();
                List<HALL> halls = hallLogic.GetEntitiesBy(p => p.Active);

                if (halls == null || halls.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> selectItemList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                selectItemList.Add(list);

                foreach (HALL hall in halls)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = hall.Id.ToString();
                    selectList.Text = hall.Name;

                    selectItemList.Add(selectList);
                }

                return selectItemList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateSessionSelectListItem()
        {
            try
            {
                SessionLogic sessionLogic = new SessionLogic();
                List<SESSION> sessions = sessionLogic.GetActiveSessions();

                if (sessions == null || sessions.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> selectItemList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = SelectSession;
                selectItemList.Add(list);

                foreach (SESSION session in sessions)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = session.Id.ToString();
                    selectList.Text = session.Name;

                    selectItemList.Add(selectList);
                }

                return selectItemList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SESSION> GetAllSessions()
        {
            try
            {
                SessionLogic sessionLogic = new SessionLogic();
                List<SESSION> sessions = sessionLogic.GetAll();

                if (sessions != null && sessions.Count > 0)
                {
                    sessions.Insert(0, new SESSION() { Id = 0, Name = SelectSession });
                }

                return sessions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<LEVEL> GetAllLevels()
        {
            try
            {
                LevelLogic levelLogic = new LevelLogic();
                List<LEVEL> levels = levelLogic.GetAll();

                if (levels != null && levels.Count > 0)
                {
                    levels.Insert(0, new LEVEL() { Id = 0, Name = SelectLevel });
                }

                return levels;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<PROGRAMME> GetAllProgrammes()
        {
            try
            {
                ProgrammeLogic programmeLogic = new ProgrammeLogic();
                List<PROGRAMME> programmes = programmeLogic.GetAll();

                if (programmes != null && programmes.Count > 0)
                {
                    //programmes.Add(new Programme() { Id = -100, Name = "All" });
                    programmes.Insert(0, new PROGRAMME() { Id = 0, Name = SelectProgramme });
                }

                return programmes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateMonthSelectListItem()
        {
            try
            {
                List<Value> months = new List<Value>();
                Value january = new Value() { Id = 1, Name = "January" };
                Value february = new Value() { Id = 2, Name = "February" };
                Value march = new Value() { Id = 3, Name = "March" };
                Value april = new Value() { Id = 4, Name = "April" };
                Value may = new Value() { Id = 5, Name = "May" };
                Value june = new Value() { Id = 6, Name = "June" };
                Value july = new Value() { Id = 7, Name = "July" };
                Value august = new Value() { Id = 8, Name = "August" };
                Value september = new Value() { Id = 9, Name = "September" };
                Value october = new Value() { Id = 10, Name = "October" };
                Value november = new Value() { Id = 11, Name = "November" };
                Value december = new Value() { Id = 12, Name = "December" };

                months.Add(january);
                months.Add(february);
                months.Add(march);
                months.Add(april);
                months.Add(may);
                months.Add(june);
                months.Add(july);
                months.Add(august);
                months.Add(september);
                months.Add(october);
                months.Add(november);
                months.Add(december);
                              
                List<SelectListItem> monthList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = "--MM--";
                monthList.Add(list);

                foreach (Value month in months)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = month.Id.ToString();
                    selectList.Text = month.Name;

                    monthList.Add(selectList);
                }

                return monthList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateDaySelectListItem(Value month, Value year)
        {
            try
            {
                List<Value> days = GetNumberOfDaysInMonth(month, year);

                List<SelectListItem> dayList = new List<SelectListItem>();
                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = "--DD--";

                dayList.Add(list);
                foreach (Value day in days)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = day.Id.ToString();
                    selectList.Text = day.Name;

                    dayList.Add(selectList);
                }

                return dayList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Value> GetNumberOfDaysInMonth(Value month, Value year)
        {
            try
            {
                int noOfDays = DateTime.DaysInMonth(year.Id, month.Id);
                List<Value> days = CreateNumberListFrom(1, noOfDays);
                return days;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static bool IsDateInTheFuture(DateTime date)
        {
            try
            {
                TimeSpan difference = DateTime.Now - date;
                if (difference.Days <= 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsStartDateGreaterThanEndDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                TimeSpan difference = endDate - startDate;
                if (difference.Days <= 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsDateOutOfRange(DateTime startDate, DateTime endDate, int noOfDays)
        {
            try
            {
                TimeSpan difference = endDate - startDate;
                if (difference.Days < noOfDays)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public static string Encrypt(string encrypData)
        {
            string data = "";
            try
            {
                string CharData = "";
                string ConChar = "";
                for (int i = 0; i < encrypData.Length; i++)
                {
                    CharData = Convert.ToString(encrypData.Substring(i, 1));
                    ConChar = char.ConvertFromUtf32(char.ConvertToUtf32(CharData, 0) + 128);
                    data = data + ConChar;

                }

            }
            catch (Exception ex)
            {
                data = "1";
                return data;
            }
            return data;


        }

        public static string Decrypt(string encrypData)
        {
            string data = "";
            try
            {
                string CharData = "";
                string ConChar = "";
                for (int i = 0; i < encrypData.Length; i++)
                {
                    CharData = Convert.ToString(encrypData.Substring(i, 1));
                    ConChar = char.ConvertFromUtf32(char.ConvertToUtf32(CharData, 0) - 128);
                    data = data + ConChar;

                }

            }
            catch (Exception ex)
            {
                data = "1";
                return data;
            }
            return data;


        }
        
        public static List<SelectListItem> PopulateStaffSelectListItem()
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                List<USER> users = userLogic.GetEntitiesBy(p => p.Role_Id != (int)Roles.Student).OrderBy(a => a.Username).ToList();
                if (users == null || users.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> userList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                userList.Add(list);

                foreach (USER user in users)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = user.Id.ToString();
                    selectList.Text = user.Username;

                    userList.Add(selectList);
                }

                return userList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateRoleSelectListItem()
        {
            try
            {
                RoleLogic roleLogic = new RoleLogic();
                List<ROLE> roles = roleLogic.GetEntitiesBy(r => r.Active);
                if (roles == null || roles.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> roleList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                roleList.Add(list);

                foreach (ROLE role in roles)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = role.Id.ToString();
                    selectList.Text = role.Name;

                    roleList.Add(selectList);
                }

                return roleList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateMenuGroupSelectListItem()
        {
            try
            {
                MenuGroupLogic menuGroupLogic = new MenuGroupLogic();
                List<MENU_GROUP> menuGroups = menuGroupLogic.GetAll();
                if (menuGroups == null || menuGroups.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> MenuGroupList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                MenuGroupList.Add(list);

                foreach (MENU_GROUP role in menuGroups)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = role.Menu_Group_Id.ToString();
                    selectList.Text = role.Name;

                    MenuGroupList.Add(selectList);
                }

                return MenuGroupList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateMenuSelectListItem()
        {
            try
            {
                MenuLogic menuLogic = new MenuLogic();
                List<MENU> menuList = menuLogic.GetAll();
                if (menuList == null || menuList.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> MenuList = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                MenuList.Add(list);

                foreach (MENU menu in menuList)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = menu.Menu_Id.ToString();
                    selectList.Text = menu.Display_Name + ", In " + menu.MENU_GROUP.Name;

                    MenuList.Add(selectList);
                }

                return MenuList.OrderBy(m => m.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateStaffTypeSelectListItem()
        {
            try
            {
                StaffTypeLogic staffTypeLogic = new StaffTypeLogic();
                List<STAFF_TYPE> staffTypeList = staffTypeLogic.GetEntitiesBy(s => s.Active);
                if (staffTypeList == null || staffTypeList.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> staffTypeListItem = new List<SelectListItem>();

                SelectListItem list = new SelectListItem();
                list.Value = "";
                list.Text = Select;
                staffTypeListItem.Add(list);

                foreach (STAFF_TYPE staffType in staffTypeList)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Value = staffType.Id.ToString();
                    selectList.Text = staffType.Name;

                    staffTypeListItem.Add(selectList);
                }

                return staffTypeListItem.OrderBy(m => m.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<SelectListItem> PopulateCourseSelectListItem()
        {
            try
            {
                CourseLogic courseLogic = new CourseLogic();
                List<COURSE> courses = courseLogic.GetEntitiesBy(p => p.Active).DistinctBy(s => s.Code).ToList();

                if (courses.Count <= 0)
                {
                    return new List<SelectListItem>();
                }

                List<SelectListItem> courselist = new List<SelectListItem>();
                if (courses.Count > 0)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = "";
                    list.Text = Select;
                    courselist.Add(list);

                    foreach (COURSE course in courses)
                    {
                        SelectListItem selectList = new SelectListItem();
                        selectList.Value = course.Id.ToString();
                        selectList.Text = course.Name;
                        courselist.Add(selectList);
                    }
                }

                return courselist;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class Value
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}