using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Repository
{
    public class UserRepository:GenericRepository<Account>
    {
        public UserRepository(DemoRedisContext context) => _context = context;
    }
}
