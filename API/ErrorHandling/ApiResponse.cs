using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLitePCL;

namespace API.ErrorHandling
{
    // Each Api error response will have AT LEAST these two properties in them (they inherit from this)
    public class ApiResponse
    {

        public ApiResponse(int statusCode, string message = null) // If no error, we leave string as null
        {
            StatusCode = statusCode;
            // ?? is a Nullent Coelescent OPERATOR -> Checks if 'message' is null, if yes, we execute everything to the RIGHT of the ?? 
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }

         private string GetDefaultMessageForStatusCode(int statusCode)
        {
            // .NET 8 switch statement syntax (don't need case, can just put value we are looking for and lambda output)
            return statusCode switch 
            {
                400 => "Bad Request",
                401 => "Unathorized Request",
                404 => "Not Found",
                500 => "Internal server error. Server could not fulfil request",
                _ => null // use _ for default 
            };
        }
    }
}