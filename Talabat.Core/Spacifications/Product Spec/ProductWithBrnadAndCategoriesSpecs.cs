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
		public ProductWithBrnadAndCategoriesSpec():base()
		{ 
			Includes.Add(P =>P.Category);
			Includes.Add(P => P.Brand);
		}
        public ProductWithBrnadAndCategoriesSpec(int id):base(P =>P.Id ==id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }
    }
}
