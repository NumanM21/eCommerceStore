using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Use to set up errors responses from our API, allow for better error handling
    public class BugErrorController : BaseApiController
    {
        private readonly StoreContext _storeContext;

        public BugErrorController(StoreContext storeContext)
        {
            _storeContext = storeContext;
            
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFoundRequest() 
        {
            var item = _storeContext.Products.Find(42);

            if (item == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok();

        }

        [HttpGet("server-error")]
        public ActionResult GetServerError() 
        {
            var item = _storeContext.Products.Find(42);

            var itemToReturn = item.ToString(); // Should be an exception, since we know item 42 doesn't exist, so item will be null

            return Ok();

        }

        [HttpGet("bad-request")] // 400
        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));

        }

        [HttpGet("bad-request/{id}")] // Another type of 400 error, due to validation (if we pass string into this rather than a int)
        public ActionResult GetNotFoundRequest(int id) 
        {

            return Ok();

        }
        
    }
}