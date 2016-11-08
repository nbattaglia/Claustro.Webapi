using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}
