
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {

        public static async Task SeedAsync(StoreContext context) // Read JSON files, and deserialize into C# objects and then add to DB
        {
            /// Order important, our product is dependent on brands and types!
            // Brands
            if(!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json"); // Calling method from API folder, so have to go up a level (to solution), then into Data and this file
                // Deserialize our brandsdata seed json, into a LIST of product brand entities we can read
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                // Tracking our entities -> hence we don't use AddRangeAsync
                context.ProductBrands.AddRange(brands); 
            }
            // Types
            if(!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json"); 
                
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
               
                context.ProductTypes.AddRange(types); 
            }
            // Products
            if(!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json"); // Calling method from API folder, so have to go up a level (to solution), then into Data and this file
                
                // Deserialize our brandsdata seed json, into a LIST of product brand entities we can read
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                // Tracking our entities 
                context.Products.AddRange(products); 
            }

            // If any changes, it will save to our DB asynchronously 
            if(context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();



        }
        
    }
}