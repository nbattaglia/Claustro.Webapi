using Ejemplo.Booter;
using Ejemplo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo_Application
{
    public interface IApplication { } //por si se necesita un app service crudo

    public interface IApplication<T> where T : class
    {

        T SaveOrUpdate(T entity);
        IQueryable<T> GetAll();
        T Get(Guid id);
        void Delete(Guid id);
        
        }

    public class Application<T> : IApplication<T> where T : Entity 
    {
        protected IRepository<T> Repository { get; set; }
        public Application(IRepository<T> repository)
        {
            Repository = repository;
        }


        public T SaveOrUpdate(T entity)
        {
            Repository.SaveOrUpdate(entity);
            return entity;
        }

        public IQueryable<T> GetAll()
        {
            return Repository.GetAll();
        }

        

        public T Get(Guid id)
        {
            if (id == null) return null;
            return Repository.Get(id);
        }

     

       


        public void Delete(Guid id)
        {
            
            Repository.Delete(id);
        }

       
    }
}
