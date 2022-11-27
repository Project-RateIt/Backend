using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.ViewedProductRepository;

public interface IViewedProductRepository : IBaseRepository<ViewedProduct>
{
    Task<List<ViewedProductDTO>> GetViewedProducts(Guid userId, int page, int pageSize,
        CancellationToken cancellationToken);
}