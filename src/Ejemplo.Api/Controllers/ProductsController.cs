using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ejemplo_Application;
using Ejemplo.Entities;
using Ejemplo.DTO;

namespace Ejemplo.Api.Controllers
{
  
    public class ProductsController : BaseController
    {
        IProductApplication _app;
        public ProductsController(IProductApplication app)
        {
            _app = app;
        }
        

        [HttpGet]
        public IActionResult Get()
        {
           
            return Ok(_app.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_app.Get(id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody]Product entity)
        {

            return Ok(_app.SaveOrUpdate(entity));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _app.Delete(id);
        }
    }
}
