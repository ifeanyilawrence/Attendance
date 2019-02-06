using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model.Entity;
using System.Linq.Expressions;

namespace Attendance.Business
{
    public class UserLogic : BusinessBaseLogic<USER>
    {
        public bool ValidateUser(string Username, string Password)
       {
           try
           {
               Expression<Func<USER, bool>> selector = p => p.Username == Username && p.Password == Password && p.Active;
                USER UserDetails = GetEntityBy(selector);
               if (UserDetails != null && UserDetails.Password != null)
               {
                   //UpdateLastLogin(UserDetails);
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch (Exception)
           {
               throw;
           }
       }

        public bool UpdateLastLogin(USER user)
        {
            try
            {
                Expression<Func<USER, bool>> selector = p => p.Username == user.Username && p.Password == user.Password;
                USER userEntity = GetEntityBy(selector);
                if (userEntity == null || userEntity.Id <= 0)
                {
                    throw new Exception(NoItemFound);
                }

                userEntity.Username = user.Username;
                userEntity.Password = user.Password;
                userEntity.Role_Id = user.Role_Id;
                userEntity.Password_Recovery = user.Password_Recovery;
                userEntity.Active = user.Active;

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

        public bool ChangeUserPassword(USER user)
        {
            try
            {
                Expression<Func<USER, bool>> selector = p => p.Username == user.Username;
                USER userEntity = GetEntityBy(selector);
                if (userEntity == null || userEntity.Id <= 0)
                {
                    throw new Exception(NoItemFound);
                }

                userEntity.Username = user.Username;
                userEntity.Password = user.Password;
                userEntity.Role_Id = user.Role_Id;
                userEntity.Password_Recovery = user.Password_Recovery;
                userEntity.Active = user.Active;

                int modifiedRecordCount = Save();
                if (modifiedRecordCount <= 0)
                {
                    throw new Exception(NoItemModified);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public USER Update(USER model)
        {
            try
            {
                Expression<Func<USER, bool>> selector = a => a.Id == model.Id;
                USER entity = GetEntityBy(selector);

                entity.Username = model.Username;
                entity.Password = model.Password;
                entity.Active = model.Active;
                entity.Role_Id = model.Role_Id;
                entity.Password_Recovery = model.Password_Recovery;

                int modifiedRecordCount = Save();
                if (modifiedRecordCount <= 0)
                {
                    throw new Exception(NoItemModified);
                }

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

       public bool Modify(USER model)
        {
            try
            {
                Expression<Func<USER, bool>> selector = u => u.Id == model.Id;
                USER entity = GetEntityBy(selector);
                if (entity == null)
                {
                    throw new Exception(NoItemFound);
                }

                entity.Id = model.Id;
                if (model.Password != null)
                {
                    entity.Password = model.Password;  
                }
                
                if (model.Role_Id > 0)
                {
                    entity.Role_Id = model.Role_Id;
                }
                if (model.Active != null)
                {
                    entity.Active = model.Active;
                }
                if (model.Password_Recovery != null)
                {
                    entity.Password_Recovery = model.Password_Recovery;
                }

                int modifiedRecordCount = Save();
                if (modifiedRecordCount <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
