
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.PackageHelpers
{
    // Class to map our PictureUrl in our dto (from prod, to prodtoreturn, and mapping to a string (since url text))
    public class ProdUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration; // Can now access the ApiUrl we created in appsetting.development.json (to avoid hardcoding our Url)

        public ProdUrlResolver(IConfiguration configuration) 
        {
            _configuration = configuration;
            
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            // Check if we have a picture Url
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                // Property Accessor ([""]) -> Allows us to access the ApiUrl
                return _configuration["ApiUrl"] + source.PictureUrl; 

            }

            return "Picture Url in Product is empty.";
        }
    }
}