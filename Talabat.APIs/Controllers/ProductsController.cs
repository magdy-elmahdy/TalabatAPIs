using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Reposotories.Centext;

namespace Talabat.APIs.Controllers
{
 
    public class ProductsController : BaseApiController
    {
        private readonly IGenericReposotory<Product> _productRepo;

        public ProductsController(IGenericReposotory<Product> ProductRepo)
        {
            _productRepo = ProductRepo;
        }

        [HttpGet]
        public async Task<IActionResult> getProducts()
        {
            var Products = await _productRepo.getAllAsync();
            return Ok(Products);
        }
    }
}
