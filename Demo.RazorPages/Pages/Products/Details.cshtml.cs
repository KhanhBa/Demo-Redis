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
    public class DetailsModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public DetailsModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || _productBusiness == null)
            {
                return NotFound();
            }

            var product = await _productBusiness.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }
    }
}
