using Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Repository
{
    public class ProductRepository:GenericRepository<Product>
    {
        public ProductRepository(DemoRedisContext context) => _context = context;
    }
}
