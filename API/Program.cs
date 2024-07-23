using API.MethodExtensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Don't need first parameter since we used this, so .Net knows we are extending this class
builder.Services.AddApplicationServices(builder.Configuration);

// ^ Future services added to ApplicationServicesExtensions class! ^ \\

var app = builder.Build();

/// NOTE -> Exception Middleware at START of HTTP pipeline (If any process in pipeline fails, .Net auto reverses Pipeline until it reaches a exception/ error handling stage again)
/// NOTE -> To stop routing/re-routing, we create the logic for it here in the MIDDLEWARE (In the HTTP Pipeline) 
// Configure the HTTP request pipeline. --> // Will have middleware here (What happens to HTTP req in and before going out)

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/end-point-error/{0}"); // Passed to error controller (NoEndPoint Controller)

if (app.Environment.IsDevelopment()) // Swagger Middleware -> https://localhost:5001/swagger
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();// Static Files being SERVED, should be ABOVE app.UseAuthorization()

app.UseAuthorization();  

app.MapControllers(); // Register controller end points, so API knows where to send HTTP req coming in 

// Get DbContext (.service is extension of IScoped) and Migrate DB on app being run
using var scope = app.Services.CreateScope(); 
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try{
    await context.Database.MigrateAsync(); 
    // Execute seed data method -> Calling our Async method from StoreContextSeed file.
    await StoreContextSeed.SeedAsync(context);
}
catch(Exception e)
{
    logger.LogError(e, "Error occuring during migration");
}


app.Run();
