
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claustro.Domain
{
   

    public class Entity 
    {
        [BsonId]
        public Guid Id { get; set; }
        
    }
}
