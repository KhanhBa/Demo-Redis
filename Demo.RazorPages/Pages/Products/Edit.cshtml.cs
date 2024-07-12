using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Data.Models;
using Demo.Business.Redis;

namespace Demo.RazorPages.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public EditModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        [BindProperty]
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
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _productBusiness.Update(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            return (await _productBusiness.GetProductById(id) == null);
        }
    }
}
