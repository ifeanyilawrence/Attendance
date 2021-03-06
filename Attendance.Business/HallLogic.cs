﻿using Attendance.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Business
{
    public class HallLogic : BusinessBaseLogic<HALL>
    {
        public bool Modify(HALL model)
        {
            try
            {
                Expression<Func<HALL, bool>> selector = a => a.Id == model.Id;
                HALL entity = GetEntityBy(selector);
                if (entity != null && entity.Id > 0)
                {
                    entity.Name = model.Name;
                    entity.Description = model.Description;
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
