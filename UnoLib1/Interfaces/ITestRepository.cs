using UnoLib1.Entity;

namespace UnoLib1.Interfaces;

public interface ITestRepository
{
    Task<TestEntity> GetByIdAsync(Guid id);
    Task<List<TestEntity>> GetAllAsync();
    Task AddAsync(TestEntity user);
    Task UpdateAsync(TestEntity user);
    Task DeleteAsync(Guid id);
}
