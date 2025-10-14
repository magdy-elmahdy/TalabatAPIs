using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helprer
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _iConfiguration;

        public ProductPictureUrlResolver(IConfiguration IConfiguration)
        {
            _iConfiguration = IConfiguration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_iConfiguration["ApiBaseUrl"]}/{source.PictureUrl}";
            return string.Empty;
        }
    }
}
