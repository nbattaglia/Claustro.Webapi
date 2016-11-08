using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ejemplo.DTO;
using Ejemplo.Entities;
using Ejemplo.Security;
using System.Security.Authentication;
using Ejemplo_Application;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Ejemplo.Api.Controllers
{
   
    public class AuthController : BaseController
    {
        IUserApplication _userApplication;
        public AuthController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }


        private void CreateDefaults()
        {
            User u = _userApplication.GetAll().Where(x => x.Mail.Equals("admin@mail.com")).FirstOrDefault();
            if (u==null)
            {
                u = new User();
                u.Mail = "admin@mail.com";
                u.Password = "1234";
                u.Username = "admin";
                _userApplication.SaveOrUpdate(u);
            }
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_userApplication.GetAll().Where(x=>x.Username.Equals("admin")));
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
                User u = _userApplication.GetByUsernamePassword(login.Mail, login.Password);//   GetAll().Where(p => p.Mail.Equals(login.Mail) && p.Password.Equals(Security.Cryptography.MD5Hash(login.Password))).FirstOrDefault();
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
