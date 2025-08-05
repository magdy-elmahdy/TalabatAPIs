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
        public async Task<ActionResult<IEnumerable<Product>>> getProducts()
        {
            var Products = await _productRepo.getAllAsync();
            return Ok(Products);
        }

        [HttpGet("{paramter}")]
        public async Task<ActionResult<Product>> getProductById(int paramter)
        {
            var Product = await _productRepo.getAsync(paramter);
            if(Product is not null){
            return Ok(Product);

            }
            else
            {
                return NotFound(new {Message = "Can not find Product with that Id" , code = 400});
            }
            //fwefefef
        }
    }
}
