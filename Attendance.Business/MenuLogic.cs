using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model.Entity;

namespace Attendance.Business
{
    public class MenuLogic : BusinessBaseLogic<MENU>
    {
        public bool Modify(MENU menu)
        { 
            try
            {
                Expression<Func<MENU, bool>> selector = a => a.Menu_Id == menu.Menu_Id;
                MENU entity = GetEntityBy(selector);
                if (entity != null && entity.Menu_Id > 0)
                {
                    entity.Controller = menu.Controller;
                    entity.Activated = menu.Activated;
                    entity.Action = menu.Action;
                    entity.Area = menu.Area;
                    entity.Display_Name = menu.Display_Name;
                    if (menu.Menu_Group_Id > 0)
                    {
                        entity.Menu_Group_Id = menu.Menu_Group_Id; 
                    }
                    
                    int modifiedRecordCount = Save();

                    if (modifiedRecordCount > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
