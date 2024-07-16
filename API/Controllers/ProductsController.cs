
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Responsible for mapping parameters passed into methods (I.e. the "{id}") 
    [Route("api/[controller]")] // how to get to this controller (url is api/(The Controller -> products)/MethodName -> What we use to test in postman too!)
    public class ProductsController : ControllerBase
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
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var specification = new ProductsWithTypesAndBrandsSpecification();

            var products = await _repoProduct.ListAsync(specification); // This is where we 'hit' our DB to pull the relevant products 
            // Use .Select to project our sequence (list here) into a product Dto instead
            return products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList(); // ToList not running against DB, runs against products variable (stores all of our data in this var)
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url: api/product/id)
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id); 

            var product = await _repoProduct.GetEntityWithSpecification(specification);
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