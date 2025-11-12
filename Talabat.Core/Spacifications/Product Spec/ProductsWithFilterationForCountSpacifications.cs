using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Spacifications.Product_Spec
{
    public class ProductsWithFilterationForCountSpacifications :BaseSpacifications<Product>
    {
        public ProductsWithFilterationForCountSpacifications(ProductSpecParams specParams)
            :base(
                 P => (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) && (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId)
            )
        {
            
        }
    }
}
