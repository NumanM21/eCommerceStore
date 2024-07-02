

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


    }
}