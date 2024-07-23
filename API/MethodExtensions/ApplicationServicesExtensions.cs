using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using API.ErrorHandling;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.MethodExtensions
{

    /// <summary>
    /// - Extend the .services class (similar to .AddSwaggerGen() -> Not framework method, swagger extended service class to add it)
    /// - ALL extension CLASSES have to be static
    /// </summary>
    public static class ApplicationServicesExtensions
    {
        // First parameter is WHAT we are extending (since we are returning what we are returing, we use 'this')
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Moved ALL builder.Services from program.cs into this EXTENSION class (to clean up program.cs)
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(); // Service to generate Swagger -> Documents our Api folder in JSON file to generate Swagger UI
            // Have to tell it where to find our auto mapping class on start up
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<StoreContext>(opt =>
            {
                // Tell dbContext to use sqllite, and we pass connection string CONFIG we created in appsetting.dev.json 
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            }); // <> name of class used that is deriving from DbContext

            // Refine the functionality of the [ApiController] header
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                { // Can access the model state -> If we have validation error, this is added to the model state, and API controller generates and sends back this Model-State-Error which we see on Postman
                    var errorArray = actionContext.ModelState
                    .Where(err => err.Value.Errors.Count > 0) // Make sure we have some errors
                    .SelectMany(s => s.Value.Errors) // Flattens out IEnurmerable into one sequence (basically a simple array)
                    .Select(x => x.ErrorMessage).ToArray(); // Project just the error message into our array to be displayed -> Flatten out the array

                    var errorResponse = new ApiValidationError
                    {
                        ErrorsEnumerable = errorArray // Setting our flatten array with error message to our Error IEnurmeable of strings in ApiValidationError class
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            // Interface first, then the implementation class
            services.AddScoped<IProductRepository, ProductRepository>(); // AddScoped means lifetime of service is the lifetime of the HTTP req (exist until class disposed)
             // For Generic, don't have a TYPE yet, so differnet syntax to product repo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            return services;

        }

    }
}