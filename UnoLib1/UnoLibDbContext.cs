using Microsoft.EntityFrameworkCore;
using UnoLib1.Configuration;
using UnoLib1.Dao;

namespace UnoLib1;

public class UnoLibDbContext(DbContextOptions<UnoLibDbContext> options) : DbContext(options)
{
    public DbSet<UserDao> Users { get; set; }
    public DbSet<TestDao> Tests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TestConfiguration());
    }
}
