using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Not Found 
        [HttpGet("notFound")]
        public ActionResult getNotFound()
        {
            var product = _dbContext.Set<Product>().Find(100);
            if(product == null) return NotFound();
            return Ok(product);
        }


        // Server Error 
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product = _dbContext.Set<Product>().Find(100);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }

    }
} 
