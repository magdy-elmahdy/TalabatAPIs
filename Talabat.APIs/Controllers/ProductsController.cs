using System.Collections.Generic;
using System.Text.Json;
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
        private readonly IGenericReposotory<ProductBrand> _brandsRepo;
        private readonly IGenericReposotory<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericReposotory<Product> ProductRepo ,
            IGenericReposotory<ProductBrand> brandsRepo,
            IGenericReposotory<ProductCategory> categoriesRepo,
            IMapper mapper)
        {
            _productRepo = ProductRepo;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }

         

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> getProducts()
        {
            var Spect = new ProductWithBrnadAndCategoriesSpec();
            var products = await _productRepo.getAllWithSpectAsync(Spect);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
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


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.getAllAsync();
            return Ok(brands);
        }


        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoriesRepo.getAllAsync();
            return Ok(categories);
        }
    }
}
