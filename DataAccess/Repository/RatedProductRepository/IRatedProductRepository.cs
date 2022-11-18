using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.RatedProductRepository;

public interface IRatedProductRepository : IBaseRepository<RatedProduct>
{
    Task<RatedProduct?> GetByUserIdAndProductId(Guid userId, Guid productId, CancellationToken cancellationToken);
    Task<List<RatedProductDTO>> GetRatedProductsAsync(Guid userId, int page, int pageSize,
        CancellationToken cancellationToken);
    Task<bool> ExistUserProductRelationAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
}
