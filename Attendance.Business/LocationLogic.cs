using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class LocationLogic : BusinessBaseLogic<LOCATION>
    {
        public bool Modify(LOCATION model)
        {
            try
            {
                Expression<Func<LOCATION, bool>> selector = a => a.Id == model.Id;
                LOCATION entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Latitude = model.Latitude;
                    entity.Longitude = model.Longitude;
                    entity.Active = model.Active;

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
