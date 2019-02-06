
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Attendance.Model.Entity;
using System.Linq.Expressions;

namespace Attendance.Business
{
    public class ProgrammeLogic : BusinessBaseLogic<PROGRAMME>
    {
        public bool Modify(PROGRAMME programme)
        {
            try
            {
                Expression<Func<PROGRAMME, bool>> selector = p => p.Id == programme.Id;
                PROGRAMME entity = GetEntityBy(selector);

                if (entity == null)
                {
                    throw new Exception(NoItemFound);
                }

                entity.Name = programme.Name;
                entity.Description = programme.Description;

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
    }
}
