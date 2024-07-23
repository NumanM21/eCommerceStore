

using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Have to tell it where to find our auto mapping class on start up
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

builder.Services.AddDbContext<StoreContext>(opt => {

    // Tell dbContext to use sqllite, and we pass connection string CONFIG we created in appsetting.dev.json 
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

}); // <> name of class used that is deriving from DbContext

// Interface first, then the implementation class
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // AddScoped means lifetime of service is the lifetime of the HTTP req (exist until class disposed)
// For Generic, don't have a TYPE yet, so differnet syntax to product repo
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

/// NOTE -> Exception Middleware at START of HTTP pipeline (If any process in pipeline fails, .Net auto reverses Pipeline until it reaches a exception/ error handling stage again)
/// NOTE -> To stop routing/re-routing, we create the logic for it here in the MIDDLEWARE (In the HTTP Pipeline) 
// Configure the HTTP request pipeline. --> // Will have middleware here (What happens to HTTP req in and before going out)

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/end-point-error/{0}"); // Passed to error controller (NoEndPoint Controller)

if (app.Environment.IsDevelopment())
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
