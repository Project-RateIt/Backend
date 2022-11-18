using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rateit.DataAccess.Entities;

namespace rateit.DataAccess.EntityTypeConfig;

public class UserTypeConfig : IEntityTypeConfiguration<Entities.User>
{
    public void Configure(EntityTypeBuilder<Entities.User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(c => c.ActivateCode).WithOne(c => c.User).HasForeignKey<ActivateCode>(c => c.UserId);
        builder.HasMany(c => c.NotedProducts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        builder.HasMany(c => c.RatedProducts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        builder.HasMany(c => c.ViewedProducts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
    }
}