using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Attendance.Model.Entity;
using System.Linq.Expressions;

namespace Attendance.Business
{
    public class SessionLogic : BusinessBaseLogic<SESSION>
    {
        public bool Modify(SESSION session)
        {
            try
            {
                Expression<Func<SESSION, bool>> selector = s => s.Id == session.Id;
                SESSION entity = GetEntityBy(selector);

                if (entity == null)
                {
                    throw new Exception(NoItemFound);
                }

                entity.Name = session.Name;
                entity.Start = session.Start;
                entity.End = session.End;
                entity.Active = session.Active;

                int modifiedRecordCount = Save();
                if (modifiedRecordCount <= 0)
                {
                    throw new Exception(NoItemModified);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<SESSION> GetActiveSessions()
        {
            List<SESSION> sessions = new List<SESSION>();
            try
            {
              return  GetEntitiesBy(a => a.Active).OrderByDescending(k => k.Name).ToList();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        
    }
}

