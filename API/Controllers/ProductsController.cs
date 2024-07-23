
using API.DTOs;
using API.ErrorHandling;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _repoProduct;
        private readonly IGenericRepository<ProductBrand> _repoProductBrand;
        private readonly IGenericRepository<ProductType> _repoProductType;

        private readonly IMapper _mapper;
       
        // Now we have a GENERIC repo, each of our entity will have its OWN repo instance
        public ProductsController(
            IGenericRepository<Product> repoProducts, 
            IGenericRepository<ProductBrand> repoProductBrand, 
            IGenericRepository<ProductType> repoProductType,
            IMapper mapper) // Imapper is interface for automapper which we can use
        {
            _repoProduct = repoProducts;
            _repoProductBrand = repoProductBrand;
            _repoProductType = repoProductType;
            _mapper = mapper;
        }
        

        [HttpGet] // end points
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var specification = new ProductsWithTypesAndBrandsSpecification();

            var products = await _repoProduct.ListAsync(specification); // This is where we 'hit' our DB to pull the relevant products 
            // Use .Select to project our sequence (list here) into a product Dto instead // Using Ok() since we return IReadOnlyList
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url: api/product/id)
        [ProducesResponseType(StatusCodes.Status200OK)] // Tell Swagger status codes we can expect from method
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] // Can now see both responses on swagger -> More specific using typeof
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id); 

            var product = await _repoProduct.GetEntityWithSpecification(specification);

            // Check if product is Null (Error handling)
            if (product == null) return NotFound(new ApiResponse(404));

                // Will make properties from our repoProduct (stored in product) to our DTO to return to client!
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
            ///// TODO: Can extend these to cover future details we would have (I.e. Fuel, Transmission, etc) \\\\\\ 
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repoProductBrand.ListAllProductsAsync()); // IReadOnlyList gives error to action result, so can use Ok() to work around it
        }

        [HttpGet("types")] 
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repoProductType.ListAllProductsAsync()); 
        }



        

    }
}