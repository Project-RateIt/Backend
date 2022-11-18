using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace rateit.DataAccess.EntityTypeConfig;

public class SubcategoryTypeConfig : IEntityTypeConfiguration<Entities.Subcategory>
{
    public void Configure(EntityTypeBuilder<Entities.Subcategory> builder)
    {
        builder.ToTable("Subcategories");
        builder.HasKey(c =>  c.Id);
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.HasMany(c => c.Products).WithOne(c => c.Subcategory).HasForeignKey(c => c.SubcategoryId);
        builder.HasOne(c => c.Category).WithMany(c => c.Subcategories).HasForeignKey(c => c.CategoryId);
    }
}