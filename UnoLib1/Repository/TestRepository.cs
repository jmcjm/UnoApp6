using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnoLib1.Dao;
using UnoLib1.Entity;
using UnoLib1.Interfaces;

namespace UnoLib1.Repository;

public class TestRepository(UnoLibDbContext context, IMapper mapper) : ITestRepository
{
    public async Task<TestEntity> GetByIdAsync(Guid id)
    {
        var testDao = await context.Tests.FirstOrDefaultAsync(u => u.Id == id);
        
        if (testDao == null)
            throw new ArgumentException("User not found");
        
        return mapper.Map<TestEntity>(testDao);
    }

    public async Task<List<TestEntity>> GetAllAsync()
    {
        var users = await context.Tests.ToListAsync();
        return mapper.Map<List<TestEntity>>(users);
    }

    public async Task AddAsync(TestEntity user)
    {
        var testDao = mapper.Map<TestDao>(user);
        await context.Tests.AddAsync(testDao);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TestEntity user)
    {
        var testDao = await context.Tests.FirstOrDefaultAsync(u => u.Id == user.Id);
        
        if (testDao == null)
            throw new ArgumentException("User not found");
        
        mapper.Map(testDao, user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Tests.FindAsync(id);
        if (user != null)
        {
            context.Tests.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
