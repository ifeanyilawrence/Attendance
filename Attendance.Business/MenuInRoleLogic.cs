using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model.Entity;

namespace Attendance.Business
{
    public class MenuInRoleLogic : BusinessBaseLogic<MENU_IN_ROLE>
    {
        public bool Modify(MENU_IN_ROLE menuInRole)
        {
            try
            {
                Expression<Func<MENU_IN_ROLE, bool>> selector = a => a.Menu_In_Role_Id == menuInRole.Menu_In_Role_Id;
                MENU_IN_ROLE entity = GetEntityBy(selector);
                if (entity != null && entity.Menu_Id > 0)
                {
                    entity.Activated = menuInRole.Activated;
                    if (menuInRole.Menu_Id > 0)
                    {
                        entity.Menu_Id = menuInRole.Menu_Id;
                    }
                    if (menuInRole.Role_Id > 0)
                    {
                        entity.Role_Id = menuInRole.Role_Id;
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
