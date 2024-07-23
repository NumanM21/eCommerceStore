using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ErrorHandling
{
    public class ApiExceptionError : ApiResponse
    {
        // If we get an exception, we want to show the stack trace (ONLY IN DEVELOPMENT, not in prod) -> Handle this logic in MIDDLEWARE class (in API proj)
        public ApiExceptionError(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            stackTraceDetails = details;
        }

        public string stackTraceDetails { get; set; }

    }
}