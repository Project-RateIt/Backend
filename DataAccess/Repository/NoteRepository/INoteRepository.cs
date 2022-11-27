using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.NoteRepository;

public interface INoteRepository : IBaseRepository<NotedProduct>
{
    void RemoveByProductAndUserId(Guid productId, Guid userId);

    Task<List<NotedProductDTO>> GetByProductAndUserId(Guid userId, int page, int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistUserProductRelationAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
}