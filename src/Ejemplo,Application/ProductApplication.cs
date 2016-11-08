using Ejemplo.Booter;
using Ejemplo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo_Application
{
    public interface IProductApplication : IApplication<Product>
    {
    }

    public class ProductApplication: Application<Product>, IProductApplication
    {
        public ProductApplication(IRepository<Product> repository) : base(repository) { }
    }
}
