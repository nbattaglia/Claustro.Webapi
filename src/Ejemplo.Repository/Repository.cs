using Ejemplo.Booter;
using Ejemplo.Entities;
using Ejemplo.MongoProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Repository
{
   


    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IMongoSessionProvider<T> _session;
        public Repository(IMongoSessionProvider<T> session)
        {
            _session= session;
        }

        public void Delete(Guid id)
        {
            _session.Delete(id);
        }


        

        public T Get(Guid id)
        {
            T o = _session.Get(id);
            
            return o;
        }

        
        public IQueryable<T> GetAll()
        {
            return _session.GetAll();
        }
          
  
        


        public T SaveOrUpdate(T entity)
        {
            _session.SaveOrUpdate(entity);
            return entity;
        }


    }
}
