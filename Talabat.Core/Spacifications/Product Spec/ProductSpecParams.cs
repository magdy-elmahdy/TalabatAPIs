using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Spacifications.Product_Spec
{
    public class ProductSpecParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        private int pageSize=5;
        public int PageSize {
            get { return pageSize; }
            set { pageSize = value>10?10:value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Search { get; set; }

    }
}
