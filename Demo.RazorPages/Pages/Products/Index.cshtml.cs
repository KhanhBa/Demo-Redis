using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Demo.Data.Models;
using Demo.Business.Redis;

namespace Demo.RazorPages.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public IndexModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {

            if (_productBusiness != null)
            {
                Product = await _productBusiness.GetProducts();
            }
        }
    }
}
