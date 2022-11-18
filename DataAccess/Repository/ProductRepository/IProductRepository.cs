using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.ProductRepository;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<ProductDTO>> Search(string query, int page, int pageSize, CancellationToken cancellationToken);
    Task<Product?> GetByEan(string ean, CancellationToken cancellationToken);
    Task<List<ProductDTO>> GetSubcategoryRanking(Guid subcategoryId, int page, int pageSize, CancellationToken cancellationToken);
    Task<bool> ExistsEanAsync(string ean, CancellationToken cancellationToken);
}