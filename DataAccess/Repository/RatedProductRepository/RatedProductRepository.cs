using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DataAccess.Repository.ProductRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.RatedProductRepository;

public class RatedProductRepository : BaseRepository<RatedProduct>, IRatedProductRepository
{
    public RatedProductRepository(DbSet<RatedProduct>? entities) : base(entities)
    {
    }

    public Task<RatedProduct?> GetByUserIdAndProductId(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        return _entities.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId, cancellationToken);
    }

    public Task<List<RatedProductDTO>> GetRatedProductsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        return _entities
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Rate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new RatedProductDTO()
            {
                Rate = c.Rate,
                ProductId = c.ProductId,
                Product = c.Product
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistUserProductRelationAsync(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        return _entities.AnyAsync(c => c.UserId == userId && c.ProductId == productId, cancellationToken);
    }
}