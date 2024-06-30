

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(opt => {

    // Tell dbContext to use sqllite, and we pass connection string CONFIG we created in appsetting.dev.json 
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

}); // <> name of class used that is deriving from DbContext

var app = builder.Build();

// Configure the HTTP request pipeline. --> // Will have middleware here (What happens to HTTP req in and before going out)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers(); // Register controller end points, so API knows where to send HTTP req coming in 

app.Run();
