using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Helprer;
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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> getProducts([FromQuery] ProductSpecParams specParams)
        {
            var Specs = new ProductWithBrnadAndCategoriesSpec(specParams);
            var products = await _productRepo.getAllWithSpectAsync(Specs);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpacs = new ProductsWithFilterationForCountSpacifications(specParams);
            var count = await _productRepo.GetCountAsync(countSpacs);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize , specParams.PageIndex , count, data) {});
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> getProductById(int id)
        {
            var spec = new ProductWithBrnadAndCategoriesSpec(id);
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
