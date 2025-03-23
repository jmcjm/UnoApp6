using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnoLib1.Dao;
using UnoLib1.Entity;
using UnoLib1.Interfaces;

namespace UnoLib1.Repository;

public class UserRepository(UnoLibDbContext context, IMapper mapper) : IUserRepository
{
    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        var userDao = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (userDao == null)
            throw new ArgumentException("User not found");
        
        return mapper.Map<UserEntity>(userDao);
    }

    public async Task<List<UserEntity>> GetAllAsync()
    {
        var users = await context.Users.ToListAsync();
        return mapper.Map<List<UserEntity>>(users);
    }

    public async Task AddAsync(UserEntity user)
    {
        var userDao = mapper.Map<UserDao>(user);
        await context.Users.AddAsync(userDao);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        var userDao = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        
        if (userDao == null)
            throw new ArgumentException("User not found");
        
        mapper.Map(userDao, user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
