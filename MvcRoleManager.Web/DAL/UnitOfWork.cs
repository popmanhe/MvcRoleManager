using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MvcRoleManager.Web.DAL
{
    public class UnitOfWork: IDisposable
    {
        protected DbContext context;
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public GenericRepository<T> Repository<T>()  
            where T: class

        {
            Type interfaceType = typeof(T);
            GenericRepository<T> returnInterface;

            if (repositories.ContainsKey(interfaceType))
            {
                returnInterface = (GenericRepository<T>)repositories[interfaceType];
            }
            else
            {
                returnInterface = new  GenericRepository<T>(context);

                repositories[interfaceType] = (GenericRepository<T>)returnInterface;
            }

            return returnInterface;

        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
