
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // how to get to this controller (url is api/products.. -> What we use to test in postman too!)
    public class ProductsController : ControllerBase
    {
        
        private readonly IProductRepository _productRepository;
        
        public ProductsController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;  
        }

        [HttpGet] // end points
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            
            return Ok(products);
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url api/product/id)
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

    }
}