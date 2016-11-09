using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using System.Security.Authentication;
using Claustro.MongoRepository;
using Claustro.Domain;
using Claustro.Webapi;
using Claustro.Security;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Ejemplo.Api.Controllers
{
   
    public class AuthController : BaseController
    {
        IMongoRepository<User> _users;
        public AuthController(IMongoRepository<User> users)
        {
            _users = users;
        }


        private void CreateDefaults()
        {
            User u = _users.GetAll().Where(x => x.Mail.Equals("admin@mail.com")).FirstOrDefault();
            if (u==null)
            {
                u = new User();
                u.Mail = "admin@mail.com";
                u.Password = "1234";
                u.Username = "Admin";
                u.Rol = Rol.Admin;
                _users.SaveOrUpdate(u);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult All()
        {
            return Ok(_users.GetAll());
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Token([FromBody]LoginDTO login)
        {
            string token;
            object response;

            CreateDefaults();



            try
            {
                User u = _users.GetAll().Where(p => p.Mail.Equals(login.Mail) && p.Password.Equals(Cryptography.Hash(login.Password))).FirstOrDefault();
                if (u == null)
                    throw new InvalidCredentialException();

                


                token = TokenHandler.GenerateToken(u);
                response = new { authenticated = true, token = token, id = u.Id.ToString() };
            }
            catch (InvalidCredentialException)
            {
                response = new { authenticated = false };

                HttpContext.Response.StatusCode = 401;
                return new ObjectResult(response);

                

            }

            

            return Ok(response);
        }




    }
}
