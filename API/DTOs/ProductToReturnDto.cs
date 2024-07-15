
namespace API.DTOs
{
    // Used for moving DATA between layers -> Don't have business logic (just setters and getters)

    // NOTE: THESE ARE TYPE SENSITIVE FOR FRONTEND (TS)
    public class ProductToReturnDto // This is what the CLIENT will receive (shaping data to be more presentable, Entities/Product.cs is what we use in backend)
    {
        public int Id { get; set; }

         public string Name { get; set; }
        
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public string ProductType { get; set; } 

        public string ProductBrand { get; set; } 
        
    }
}