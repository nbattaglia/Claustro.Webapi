using Claustro.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claustro.MongoRepository
{
    public interface IMongoRepository<T> where T : Entity
    {
        T Get(Guid id);
        IQueryable<T> GetAll();
        void SaveOrUpdate(T entity);
        void Delete(Guid id);

    }

    public class MongoRepository<T> : IMongoRepository<T> where T : Entity
    {
        Type typeParameterType = typeof(T);


        IMongoCollection<T> _collection;
        IMongoSession _session;
        public MongoRepository(IMongoSession session)
        {
            _session = session;
            _collection = _session.Db.GetCollection<T>(typeParameterType.Name);
        }

        public void Delete(Guid id)
        {

            _collection.DeleteOne(Builders<T>.Filter.Eq("Id", id));
        }

        public T Get(Guid id)
        {

            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = _collection.Find(filter).FirstOrDefault();


            return result;
        }

        public IQueryable<T> GetAll()
        {

            //var colletion = _session.Db.GetCollection<T>(typeParameterType.Name);
            return _collection.AsQueryable();

        }

        public void SaveOrUpdate(T entity)
        {
            var colletion = _session.Db.GetCollection<T>(typeParameterType.Name);
            if (entity.Id.Equals(Guid.Empty))
                colletion.InsertOne(entity);
            else
            {
                var filter = Builders<T>.Filter.Eq("Id", entity.Id);
                colletion.ReplaceOne(filter, entity);
            }

        }

    }
}
