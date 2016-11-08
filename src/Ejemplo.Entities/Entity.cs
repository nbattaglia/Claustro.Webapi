using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Entities
{
    public interface EntityWithTypedId<T>
    {
        T Id { get; set; }
    }

    public class Entity : EntityWithTypedId<Guid>
    {
        [BsonId]
        public Guid Id { get; set; }
        
    }
}
