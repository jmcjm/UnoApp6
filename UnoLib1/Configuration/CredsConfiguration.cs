using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnoLib1.Dao;

namespace UnoLib1.Configuration;

public class CredsConfiguration : IEntityTypeConfiguration<CredsDao>
{
    public void Configure(EntityTypeBuilder<CredsDao> builder)
    {
        builder.ToTable("Creds");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .IsRequired();
        
        builder.Property(u => u.Username)
            .HasColumnName("EAN")
            .IsRequired()
            .HasMaxLength(15);
        
        builder.Property(u => u.Password)
            .HasColumnName("Quantity")
            .IsRequired();
    }
}
