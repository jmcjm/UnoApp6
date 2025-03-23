using UnoLib1.Entity;

namespace UnoLib1.Interfaces;

public interface IUserRepository
{
    Task<UserEntity> GetByIdAsync(Guid id);
    Task<List<UserEntity>> GetAllAsync();
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
}
