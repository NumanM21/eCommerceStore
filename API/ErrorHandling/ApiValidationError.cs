

using Microsoft.VisualBasic;

namespace API.ErrorHandling
{
    public class ApiValidationError : ApiResponse
    {
        // Class to handle Unauthorised/ validation error -> Hence we can hard code the status code into the base(400) for the ApiResponse ctor
        public ApiValidationError() : base(400)
        {
        }

        public IEnumerable<string> ErrorsEnumerable { get; set; }

    }
}