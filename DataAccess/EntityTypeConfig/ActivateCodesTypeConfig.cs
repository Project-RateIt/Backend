using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rateit.DataAccess.Entities;

namespace rateit.DataAccess.EntityTypeConfig;

public class ActivateCodesTypeConfig : IEntityTypeConfiguration<Entities.ActivateCode>
{
    public void Configure(EntityTypeBuilder<ActivateCode> builder)
    {
        builder.ToTable("ActivateCodes");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();    
        builder.HasOne(c => c.User).WithOne(u => u.ActivateCode).HasForeignKey<ActivateCode>(c => c.UserId);
    }
}