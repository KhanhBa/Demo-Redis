using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Demo.Data.Models;
using Demo.Business.Redis;

namespace Demo.RazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public CreateModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _productBusiness == null || Product == null)
            {
                return Page();
            }

            await _productBusiness.Create(Product);
          
            return RedirectToPage("./Index");
        }
    }
}
