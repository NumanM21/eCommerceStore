using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // how to get to this controller (url is api/products.. -> What we use to test in postman too!)
    public class ProductsController : ControllerBase
    {
        
        private readonly StoreContext _context; // convention to use _ for readonly fields. This gives us access to DbContext 
        public ProductsController(StoreContext context) // This would be DEPENDENCY INJECTION (req comes into controller, framework root req to our controller, and create an instance of ProductsController
    // It will see us wanting to create a StoreContext (a service) we specified in our program class, and will create a new instance of dbcontext which our product controller can use
    // Can then use ALL methods from our dbcontext. When req is done, products controller disposed and storecontext removed (Memory management not needed here!))
        {
            this._context = context;
            
        }

        [HttpGet] // end points
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            
            return products;
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url api/product/id)
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }

    }
}