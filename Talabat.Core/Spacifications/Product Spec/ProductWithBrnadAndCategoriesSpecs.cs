using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Spacifications.Product_Spec
{
	public class ProductWithBrnadAndCategoriesSpec :BaseSpacifications<Product>
	{
		public ProductWithBrnadAndCategoriesSpec(ProductSpecParams specParams)
			: base( P => 
				(!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
				(!specParams.CategoryId.HasValue ||P.CategoryId == specParams.CategoryId) &&
				(string.IsNullOrEmpty(specParams.Search)||P.Name.ToLower().Contains(specParams.Search))
            )
		{ 
			//Include
			Includes.Add(P =>P.Category);
			Includes.Add(P => P.Brand);
			//Sort
			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "priceAsc":
						OrderBy = P => P.Price; break;
					case "priceDesc":
						OrderByDesc = P => P.Price;	break;
					default:
						OrderBy = P =>P.Name; break;
				}
			}
			else
			{
				OrderBy = P => P.Name;
			}
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

        public ProductWithBrnadAndCategoriesSpec(int id):base(P =>P.Id ==id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }
    }
}
