

using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{

    // Add classes we want to be able to query here as DbSet<>
    public class StoreContext : DbContext
    {
        
        public StoreContext(DbContextOptions options) : base(options) 
        {}

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

            // Change Decimal to DOUBLE (double support by SQLite -> Prevent 500 error)
            if (Database.ProviderName == "Microsoft.EntityFrameWorkCore.Sqlite")
            {
                // This conversion ONLY happens when Sqlite is RETURNING a decimal value BACK (will convert to double at that point)

                // Loop through the entities in our modelbuilder (ALL, including the ones which already are a double/ int)
                foreach(var entityToReturn in modelBuilder.Model.GetEntityTypes()) // NOTE: modelBuilder is an API in EF Core 
                {
                    // Find all properties of the entity in  which they have the type of decimal in our modelBuilder
                    var prop = entityToReturn.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));

                    // Loop through all entities which ARE type of DECIMAL and convert to double
                    foreach(var property in prop)
                    {
                        modelBuilder.Entity(entityToReturn.Name) // Gets the name of the entity type, which is a string representing the CLR class name.
                        .Property(property.Name) // Specifies the property within the entity type that should be configured. Is the name of the property as a string.
                        .HasConversion<double>();
                    }
                }

            }
        }


    }
}