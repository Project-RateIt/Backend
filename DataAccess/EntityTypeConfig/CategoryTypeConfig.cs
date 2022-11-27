using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace rateit.DataAccess.EntityTypeConfig;

public class CategoryTypeConfig : IEntityTypeConfiguration<Entities.Category>
{
    public void Configure(EntityTypeBuilder<Entities.Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.HasMany(c => c.Subcategories).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}