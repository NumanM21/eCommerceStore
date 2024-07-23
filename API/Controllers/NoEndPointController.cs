using API.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Override Root we get from BaseApiController (As we need to handle bad url requests error)
    [Route("end-point-error/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // Tells Swagger to ignore this controller -> Since no Get, we get fetch error
    public class NoEndPointController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));

        }
        
    }
}