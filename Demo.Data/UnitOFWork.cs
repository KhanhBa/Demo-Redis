using Demo.Data.Models;
using Demo.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data
{
    public class UnitOFWork
    {
        private ProductRepository _productRepository;
        public UserRepository _userRepository;
        private DemoRedisContext _unitOfWorkContext;
        public UnitOFWork()
        {
            _unitOfWorkContext ??= new DemoRedisContext();
        }
        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new Repository.ProductRepository(_unitOfWorkContext);
            }
        }
        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new Repository.UserRepository(_unitOfWorkContext);
            }
        }
    }
}
