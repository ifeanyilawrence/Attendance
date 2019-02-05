using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using Attendance.Model.Entity;

namespace Attendance.Business
{
    public class RoleLogic : BusinessBaseLogic<ROLE>
    {
        public override List<ROLE> GetAll()
        {
            try
            {
                List<ROLE> roles = base.GetAll();
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ROLE Get(USER user)
        {
            try
            {
                ROLE role = null;
                if (user != null)
                {
                    Expression<Func<ROLE, bool>> selector = r => r.Id == user.ROLE.Id;
                    role = base.GetEntityBy(selector);
                }

                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Modify(ROLE role)
        {
            try
            {
                Expression<Func<ROLE, bool>> selector = r => r.Id == role.Id;
                ROLE roleEntity = GetEntityBy(selector);
                roleEntity.Name = role.Name;
                roleEntity.Description = role.Description;

                int rowsAffected = repository.Save();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception(NoItemModified);
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(ArgumentNullException);
            }
            //catch (UpdateException)
            //{
            //    throw new UpdateException(UpdateException);
            //}
            catch (Exception)
            {
                throw;
            }
        }

        public bool Remove(ROLE role)
        {
            try
            {
                Func<ROLE, bool> selector = r => r.Id == role.Id;
                bool suceeded = base.Delete(selector);

                base.repository.Save();
                return suceeded;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
