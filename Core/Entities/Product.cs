
namespace Core.Entities
{
    public class Product
    {
        // [Key] another way to tell EF this is the PK 
        public int Id { get; set; } // EF will make this the PK by default

        public string Name { get; set; }

    }
}