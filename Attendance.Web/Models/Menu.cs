using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Attendance.Business;
using Attendance.Model.Entity;

namespace Attendance.Web.Models
{
    public static class Menu
    {
        public static string GetUserRole(string userName)
        {
            string roleName = "";
            try
            {
                UserLogic userLogic = new UserLogic();
                USER user = userLogic.GetEntityBy(u => u.Username == userName);
                roleName = user.ROLE.Name;
            }
            catch (Exception)
            {   
                throw;
            }

            return roleName;
        }

        public static List<MENU> GetMenuList(string role)
        {
            List<MENU> menuList = new List<MENU>();
            try
            {
                MenuLogic menuLogic = new MenuLogic();
                MenuInRoleLogic menuInRoleLogic = new MenuInRoleLogic();

                List<MENU_IN_ROLE> menuInRoleList = menuInRoleLogic.GetEntitiesBy(m => m.ROLE.Name == role && m.Activated);
                for (int i = 0; i < menuInRoleList.Count; i++)
                {
                    MENU_IN_ROLE thisMenuInRole = menuInRoleList[i];
                    menuList.Add(thisMenuInRole.MENU);
                }
            }
            catch (Exception)
            {   
                throw;
            }

            return menuList;
        } 
    }
}