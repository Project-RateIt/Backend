using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace rateit.DataAccess.EntityTypeConfig;

public class NotedProductsTypeConfig : IEntityTypeConfiguration<Entities.NotedProduct>
{
    public void Configure(EntityTypeBuilder<Entities.NotedProduct> builder)
    {
        builder.ToTable("NotedProducts");
        builder.HasKey(c => new { c.UserId, c.ProductId });
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.HasOne(c => c.User).WithMany(c => c.NotedProducts).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.Product).WithMany(c => c.NotedProducts).HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}