using Demo.Data;
using Demo.Data.Models;
using Demo.Data.Repository;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business.Redis
{
    public interface IProductBusiness
    {
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetProducts();
        Task<Product> Update(Product product);
        Task<Product> Create(Product product);
    }
    public class ProductBusiness:IProductBusiness
    {
        private readonly RedisManagerment _redisManagerment;
        private readonly UnitOFWork _unitOfWork;
        public ProductBusiness(UnitOFWork unitOFWork, RedisManagerment redisManagerment)
    {
            _unitOfWork = unitOFWork;
            _redisManagerment = redisManagerment;
    }

        public async Task<Product> Create(Product product)
        {
            var result = await _unitOfWork.ProductRepository.CreateAsync(product);
            await _redisManagerment.AddProductToListAsync("ProductList", product);
            await _redisManagerment.PublishAsync("product-updates", $"Product {product.Name} added!");
            return result;
        }

        public async Task<Product> GetProductById(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string cacheKey = $"Product:{id}";
            string productJson = _redisManagerment.GetData(cacheKey);
            if (productJson == null)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product != null)
                {
                    productJson = JsonConvert.SerializeObject(product);
                    _redisManagerment.SetData(cacheKey, productJson);

                }
                stopwatch.Stop();
                Console.WriteLine($"Database: {stopwatch.ElapsedMilliseconds} ms");
                return product;
            }
            stopwatch.Stop();
            Console.WriteLine($"Redis: {stopwatch.ElapsedMilliseconds} ms");
            return JsonConvert.DeserializeObject<Product>(productJson);
        }
        public async Task<List<Product>> GetProducts()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string cacheKey = "ProductList";
            string productListJson = _redisManagerment.GetData(cacheKey);

            if (productListJson == null|| productListJson == "[]")
            {
                var productList = await _unitOfWork.ProductRepository.GetAllAsync();
                if (productList != null)
                {
                    productListJson = JsonConvert.SerializeObject(productList);
                    _redisManagerment.SetData(cacheKey, productListJson);
                }
                Console.WriteLine($"Database: {stopwatch.ElapsedMilliseconds} ms");
                return productList;
            }
            Console.WriteLine($"Redis: {stopwatch.ElapsedMilliseconds} ms");
            return JsonConvert.DeserializeObject<List<Product>>(productListJson);
        }

        public async Task<Product> Update(Product product)
        {
            var result = await _unitOfWork.ProductRepository.UpdateAsync(product);
            string cacheKey = $"Product:{product.Id}";
            string productJson = JsonConvert.SerializeObject(result);
            _redisManagerment.SetData(cacheKey, productJson);
            _redisManagerment.DeleteData("ProductList");
            return JsonConvert.DeserializeObject<Product>(productJson);
        }
    }
}
