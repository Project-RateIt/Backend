using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.ViewedProductRepository;

public class ViewedProductRepository : BaseRepository<ViewedProduct>, IViewedProductRepository
{
    public ViewedProductRepository(DbSet<ViewedProduct>? entities) : base(entities)
    {
    }

    public Task<List<ViewedProductDTO>> GetViewedProducts(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        return _entities.Where(c => c.UserId == userId)
            .OrderBy(c => c.Date)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new ViewedProductDTO
            {
                ProductId = c.ProductId,
                Product = c.Product,
                Date = c.Date
            })
            .ToListAsync(cancellationToken);
    }
}