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
using Talabat.Repository.Data;
using Talabat.Repository;

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


        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> AddProduct(ProductCreateDto productDto)
        {
            // 1. تحقق من صحة البيانات
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. تحويل الـ DTO إلى Entity
            var product = _mapper.Map<Product>(productDto);

            // 3. إضافة المنتج إلى الـ DbContext
            await _productRepo.AddAsync(product);

            // 4. حفظ التغييرات (SaveChanges)
            // لو مافيش UnitOfWork فهتحتاج تمرر الـ DbContext نفسه في الريبو وتستدعي SaveChanges
            var context = (StoreContext)typeof(GenericReposotory<Product>)
                          .GetField("_storeContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                          .GetValue(_productRepo);
            await context.SaveChangesAsync();

            // 5. تحويل الناتج إلى DTO للرجوع به
            var productToReturn = _mapper.Map<ProductToReturnDto>(product);

            // 6. رجع استجابة 201 Created
            return CreatedAtAction(nameof(getProductById), new { id = product.Id }, productToReturn);
        }
    }
}
