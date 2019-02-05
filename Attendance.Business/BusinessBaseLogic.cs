
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Attendance.Data;
using System.Linq.Expressions;
using System.Data.Entity.Validation;

namespace Attendance.Business
{
    public abstract class BusinessBaseLogic<E> : IDisposable where E : class
    {
        protected IRepository repository = new Repository();
        
        protected const string ArgumentNullException = "Null object argument. Please contact your system administrator";
        protected const string UpdateException = "Operation failed due to update exception!";
        protected const string NoItemModified = "No item modified!";
        protected const string NoItemFound = "No item found to modified!";
        protected const string NoItemRemoved = "No item removed!";
        protected const string ErrowDuringProccesing = "Error Occurred During Processing.";
        protected const string ContainsDuplicate = "Error Occurred, the data being requested contains duplicates, Please try again or contact ICT";

        public virtual E GetEntityBy(Expression<Func<E, bool>> selector)
        {
            try
            {
                return repository.GetSingleBy(selector);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                {
                    if (String.Equals(ex.Message, "Sequence contains more than one element"))
                    {
                        throw new InvalidOperationException(ContainsDuplicate);
                    }
                }
                else if (ex.GetType() == typeof(NullReferenceException))
                {
                    if (String.Equals(ex.Message, "Object reference not set to an instance of an object."))
                    {
                        throw new NullReferenceException(ArgumentNullException);
                    }
                }

                throw ex;
            }
        }

        public virtual List<E> GetEntitiesBy(Expression<Func<E, bool>> selector = null, Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null, string includeProperties = "")
        {
            try
            {
                return repository.GetBy(selector, orderBy, includeProperties).ToList();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                {
                    if (String.Equals(ex.Message, "Sequence contains more than one element"))
                    {
                        throw new InvalidOperationException(ContainsDuplicate);
                    }
                }
                else if (ex.GetType() == typeof(NullReferenceException))
                {
                    if (String.Equals(ex.Message, "Object reference not set to an instance of an object."))
                    {
                        throw new NullReferenceException(ArgumentNullException);
                    }
                }

                throw ex;
            }
        }
        public virtual List<E> GetAll()
        {
            try
            {
                return repository.GetAll<E>().ToList();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                {
                    if (String.Equals(ex.Message, "Sequence contains more than one element"))
                    {
                        throw new InvalidOperationException(ContainsDuplicate);
                    }
                }
                else if (ex.GetType() == typeof(NullReferenceException))
                {
                    if (String.Equals(ex.Message, "Object reference not set to an instance of an object."))
                    {
                        throw new NullReferenceException(ArgumentNullException);
                    }
                }

                throw ex;
            }
        }

        public virtual int Create(E entity)
        {
            try
            {
                E addedEntity = repository.Add(entity);

                return repository.Save();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(ArgumentNullException);
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        public virtual int Create(List<E> entities)
        {
            try
            {
                repository.Add<E>(entities);

                return repository.Save();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(ArgumentNullException);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                {
                    if (String.Equals(ex.Message, "Sequence contains more than one element"))
                    {
                        throw new InvalidOperationException(ContainsDuplicate);
                    }
                }
                else if (ex.GetType() == typeof(NullReferenceException))
                {
                    if (String.Equals(ex.Message, "Object reference not set to an instance of an object."))
                    {
                        throw new NullReferenceException(ArgumentNullException);
                    }
                }

                throw ex;
            }
        }

        //public virtual bool Modify(T model)
        //{
        //    try
        //    {
        //        E entity = ModifyHelper(model);
        //        if (entity == null)
        //        {
        //            throw new Exception(NoItemFound);
        //        }

        //        int modifiedRecordCount = Save();
        //        if (modifiedRecordCount <= 0)
        //        {
        //            throw new Exception(NoItemModified);
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //protected abstract E ModifyHelper(T model);

        public bool Delete(Expression<Func<E, bool>> selector)
        {
            try
            {
                repository.Delete(selector);
                return Save() > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(object id)
        {
            try
            {
                repository.Delete(id);
                return Save() > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public int Save()
        {
            return repository.Save();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (repository != null)
                {
                    repository.Dispose();
                    repository = null;
                }
            }
        }

        public bool Delete(List<E> entity)
        {
            try
            {
                repository.Delete(entity);
                return Save() > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
