using UnoLib1.Entity;

namespace UnoLib1.Interfaces;

public interface ICredsRepository
{
    Task<CredsEntity?> GetAsync();
    Task AddAsync(CredsEntity creds);
    Task DeleteAsync();
}
