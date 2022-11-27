using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace rateit.DataAccess.EntityTypeConfig;

public class ViewedProductsTypeConfig : IEntityTypeConfiguration<Entities.ViewedProduct>
{
    public void Configure(EntityTypeBuilder<Entities.ViewedProduct> builder)
    {
        builder.ToTable("ViewedProducts");
        builder.HasKey(c => new { c.UserId, c.ProductId });

        builder.HasOne(c => c.User).WithMany(c => c.ViewedProducts).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.Product).WithMany(c => c.ViewedProducts).HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}