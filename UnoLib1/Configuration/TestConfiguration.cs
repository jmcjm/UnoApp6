using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnoLib1.Dao;

namespace UnoLib1.Configuration;

public class TestConfiguration : IEntityTypeConfiguration<TestDao>
{
    public void Configure(EntityTypeBuilder<TestDao> builder)
    {
        builder.ToTable("Test");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .IsRequired();
        
        builder.Property(u => u.Ean)
            .HasColumnName("EAN")
            .IsRequired()
            .HasMaxLength(15);
        
        builder.Property(u => u.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();
    }
}
