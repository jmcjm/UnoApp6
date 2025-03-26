using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnoLib1.Dao;
using UnoLib1.Entity;
using UnoLib1.Interfaces;

namespace UnoLib1.Repository;

public class CredsRepository(UnoLibDbContext context, IMapper mapper) : ICredsRepository
{
    public async Task<CredsEntity?> GetAsync()
    {
        var users = await context.Creds.FirstOrDefaultAsync();
        return users == null ? null : mapper.Map<CredsEntity>(users);
    }

    public async Task AddAsync(CredsEntity creds)
    {
        var credsDao = mapper.Map<CredsDao>(creds);
        await context.Creds.AddAsync(credsDao);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync()
    {
        context.Creds.RemoveRange(context.Creds);
        await context.SaveChangesAsync();
    }
}
