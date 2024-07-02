

using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{

    // Add classes we want to be able to query here as DbSet<>
    public class StoreContext : DbContext
    {
        
        public StoreContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Product> Products { get; set; } // EF will use this name when creating the DB for this entity (Product class)

        public DbSet<ProductType> ProductTypes {get; set;}

        public DbSet<ProductBrand> ProductBrands { get; set; }

        // Will override a method which comes with DbContext so we can use our configuration for EF rather than what has been specified as default!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call from base (DbContext class) -> Allow DbContext to run and execute as normal (to avoid error before pushing our configuration)
            base.OnModelCreating(modelBuilder);
            // .GetExecutingAssembley() gets the assembley that contains the code currently executing -> Typically refers to the project where the DbContext is defined
            // -> ApplyConfigurationFromAssembley() will scan this project and classes to find IEntityConfiguration<> and automatically apply these configuration to the model
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
        }


    }
}