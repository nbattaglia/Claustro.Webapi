using AutoMapper;
using Ejemplo.DTO;
using Ejemplo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Booter
{
    public class Boot
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Product, ProductDTO>();
            });

            
        }

       public static MongoSession InitMongoDbSession()
        {
            return new MongoSession();
        }
    }
}
