using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnoLib1.Dao;

namespace UnoLib1.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> builder)
    {
        builder.ToTable("Tests");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .IsRequired();
        
        builder.Property(u => u.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.Age)
            .HasColumnName("Age")
            .IsRequired();
    }
}
