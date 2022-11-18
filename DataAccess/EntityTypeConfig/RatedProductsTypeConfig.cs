using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace rateit.DataAccess.EntityTypeConfig;

public class RatedProductsTypeConfig : IEntityTypeConfiguration<Entities.RatedProduct>
{
    public void Configure(EntityTypeBuilder<Entities.RatedProduct> builder)
    {
        builder.ToTable("RatedProducts");
        builder.HasKey(c => new {c.ProductId, c.UserId});
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.HasOne(c => c.User).WithMany(c => c.RatedProducts).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.Product).WithMany(c => c.RatedProducts).HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}