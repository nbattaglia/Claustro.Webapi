using Ejemplo.Booter;
using Ejemplo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo_Application
{
    
        public interface IUserApplication : IApplication<User>
        {
            User GetByUsernamePassword(string username, string password);
        }

        public class UserApplication : Application<User>, IUserApplication
        {
            public UserApplication(IRepository<User> repository) : base(repository) { }

        public User GetByUsernamePassword(string mail, string password)
        {
            var res = Repository.GetAll().Where(x => x.Mail.Equals(mail) && x.Password.Equals(password)).FirstOrDefault();


            return res;
        }
    }
    
}
