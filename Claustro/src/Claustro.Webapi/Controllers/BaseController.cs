using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
    }
}
