
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
       
        // Now we have a GENERIC repo, each of our entity will have its OWN repo instance
        public ProductsController(
            IGenericRepository<Product> repoProducts, 
            IGenericRepository<ProductBrand> repoProductBrand, 
            IGenericRepository<ProductType> repoProductType) 
        {
            _repoProduct = repoProducts;
            _repoProductBrand = repoProductBrand;
            _repoProductType = repoProductType;

        }

        [HttpGet] // end points
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var specification = new ProductsWithTypesAndBrandsSpecification();

            var products = await _repoProduct.ListAsync(specification);
            
            return Ok(products);
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url: api/product/id)
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id); 

            return await _repoProduct.GetEntityWithSpecification(specification);
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