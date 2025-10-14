using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helprer
{
    public class MappigProfiles : Profile
    {
        public MappigProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.BrandNme, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.CategoryName, O => O.MapFrom(S => S.Category.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

        }
    }
}
