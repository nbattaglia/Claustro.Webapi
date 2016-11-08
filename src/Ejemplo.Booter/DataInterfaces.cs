using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Booter
{
    public interface IMongoSession

    {
        IMongoDatabase Db { get; }
        IMongoClient Session { get; }

    }

    public interface IRepository<T> where T : class
    {
        //   IDbContext DbContext { get; }

        T Get(Guid id);
        IQueryable<T> GetAll();
        T SaveOrUpdate(T entity);
        void Delete(Guid id);

    }
}
