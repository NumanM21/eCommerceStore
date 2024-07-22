
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Have this class as our base controller where we place this class [] above our API rather than re-writing the same headers for API end points
    [ApiController] // Responsible for mapping parameters passed into methods (I.e. the "{id}") 
    [Route("api/[controller]")] // how to get to this controller (url is api/(The Controller -> products)/MethodName -> What we use to test in postman too!)
    public class BaseApiController : ControllerBase
    {
        
    }
}