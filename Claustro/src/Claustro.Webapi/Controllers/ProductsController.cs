using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ejemplo.Entities;
using Claustro.MongoRepository;

namespace Ejemplo.Api.Controllers
{
  
    public class ProductsController : BaseController
    {
        IMongoRepository<Product> _prods;
        public ProductsController(IMongoRepository<Product> prods)
        {
            _prods = prods;
        }
        

        [HttpGet]
        public IActionResult Get()
        {
           
            return Ok(_prods.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_prods.Get(id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        public void Put([FromBody]Product entity)
        {

            _prods.SaveOrUpdate(entity);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _prods.Delete(id);
        }
    }
}
