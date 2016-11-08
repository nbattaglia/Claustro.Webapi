using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Booter
{

    

    public class MongoSession : IMongoSession
    {

        
        private IMongoDatabase _db;
        private IMongoClient _client;
        public MongoSession()
        {
            _client = new MongoClient();
            _db = _client.GetDatabase("prueba");
        }
              

      

       IMongoClient IMongoSession.Session
        {
            get
            {
                return _client;
            }
        }

        IMongoDatabase IMongoSession.Db
        {
            get
            {
                return _db;
            }
        }
    }
}
