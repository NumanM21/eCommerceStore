
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using API.ErrorHandling;

namespace API.Middleware
{
    public class ApiExceptionMiddleware
    {
        // IHostEnvironment to check dev mode, ILogger to allow us to post a stack trace, RequestDelegate process Http req 
        private readonly RequestDelegate _nextReq;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;
        public ApiExceptionMiddleware(RequestDelegate nextReq, ILogger<ApiExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
            _nextReq = nextReq;
        }

        // The actual exception handling middleware method -> hence try catch
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {  
                // No exception, so allow http pipeline to process next req in middleware
                await _nextReq(context);
            }
            catch(Exception e)
            {

                _logger.LogError(e, e.Message);
                // Write our OWN context response to send to client (since we can't process their actual request)
                context.Response.ContentType = "application/json"; // FORMAT response in JSON 
                
                var httpStatusCode = (int)HttpStatusCode.InternalServerError; // Setting status code to be Http 500 

                context.Response.StatusCode = httpStatusCode; 

                // Check if in deve mode before responding
                var response = _environment.IsDevelopment() 
                // If we are in Development --> ToString() to help format stack trace better (uses /n more)
                ? new ApiExceptionError(httpStatusCode, e.Message, e.StackTrace.ToString()) 
                // If we are in Production
                : new ApiExceptionError(httpStatusCode);

                // Since returning JSON and NOT in a API Controller class, .Net doesn't create Json in CamelCase by default, so need to specify that
                var jsonOption = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, jsonOption); // Formats response into JSON format, with our custom formatting jsonOption

                await context.Response.WriteAsync(json);

            }

        }

    }
}