using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductDTO, Product>()
                .ReverseMap()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<PhotoDTO, Photo>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<Product, AddProductDTO>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.photos, opt => opt.Ignore());

            CreateMap<Product, UpdateProductDTO>()
           .ForMember(dest => dest.Photo, opt => opt.Ignore())
           .ReverseMap()
           .ForMember(dest => dest.photos, opt => opt.Ignore());
        }
    }
    
    
}
