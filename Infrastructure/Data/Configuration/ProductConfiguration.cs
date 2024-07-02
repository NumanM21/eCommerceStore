
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder) // Product is entity, builder parameter which we use
        {
            // Can use builder.property and then specify the expression we are looking for
            builder.Property(x => x.Id).IsRequired(); 

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100); // Makes our name field UN-NULLABLE and we can set a MAX length (so use less memory)
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)"); // Need to tell builder the column type for a decimal (not a string) (18 -> Precision (number of digits on either side of decimal) and 2 is decimal places allowed)
            builder.Property(x => x.PictureUrl).IsRequired();
            // Configure relationship (One brand can have many product)
            builder.HasOne(pb => pb.ProductBrand).WithMany().HasForeignKey(x => x.ProductBrandId); 
            builder.HasOne(pt => pt.ProductType).WithMany().HasForeignKey(x => x.ProductTypeId);

        }
    }
}