using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Repository.BaseRepository;

public interface IBaseRepository<TEntity>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQueryable();
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    void RemoveById(Guid id);
    void Remove(TEntity entity);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}