using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claustro.Domain
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }
    }
}
