using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Reposotories.Centext;
using Talabat.Core.Spacifications.Product_Spec;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericReposotory<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericReposotory<Product> ProductRepo ,IMapper mapper)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> getProducts()
        {
            var Spect = new ProductWithBrnadAndCategoriesSpec();
            var products = await _productRepo.getAllWithSpectAsync(Spect);
            return Ok(_mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(products));
        }





        [HttpGet("{paramter}")]
        public async Task<ActionResult<ProductToReturnDto>> getProductById(int paramter)
        {
            var spec = new ProductWithBrnadAndCategoriesSpec(paramter);
            var product = await _productRepo.getWithSpectAsync(spec);
            if(product is not null){
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
            }
            else
            {
                return NotFound(new {Message = "Can not find Product with that Id" , code = 400});
            }
        }
    }
}
