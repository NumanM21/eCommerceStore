
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.PackageHelpers
{
    // Class to define out automapping (which class we are going from and to) || A service, so add to Program.cs
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Where from and to in <>. Takes property names from Product and matches it to ProductToReturnDto (if they match)
            CreateMap<Product, ProductToReturnDto>();
        }
    }
}