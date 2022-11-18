using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rateit.DataAccess.Entities;

namespace rateit.DataAccess.EntityTypeConfig;

public class ProductTypeConfig : IEntityTypeConfiguration<Entities.Product>
{
    public void Configure(EntityTypeBuilder<Entities.Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.HasOne(c => c.Subcategory).WithMany(c => c.Products).HasForeignKey(c => c.SubcategoryId);
        builder.HasOne(c => c.Category).WithMany(c => c.Products).HasForeignKey(c => c.CategoryId);
        
        builder.HasMany(c => c.NotedProducts).WithOne(c => c.Product).HasForeignKey(c => c.ProductId);
        builder.HasMany(c => c.RatedProducts).WithOne(c => c.Product).HasForeignKey(c => c.ProductId);
        builder.HasMany(c => c.ViewedProducts).WithOne(c => c.Product).HasForeignKey(c => c.ProductId);
    }
}