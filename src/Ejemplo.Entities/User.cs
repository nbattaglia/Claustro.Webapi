using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
