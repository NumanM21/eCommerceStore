using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // how to get to this controller (url is api/products.. -> What we use to test in postman too!)
    public class ProductController : ControllerBase
    {
        [HttpGet] // end points
        public string GetProducts()
        {
            return "List of Products";
        }

        [HttpGet("{id}")] // need to specify the root parameter done with {}, and what we are passing in 'id' of product in this case (url api/product/id)
        public string GetProduct(int id)
        {
            return "Product";
        }

    }
}