using System.Linq;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.NoteRepository;

public class NoteRepository : BaseRepository<NotedProduct>, INoteRepository
{
    public NoteRepository(DbSet<NotedProduct>? entities) : base(entities)
    {
    }

    public void RemoveByProductAndUserId(Guid productId, Guid userId)
    {
        var entry = _entities.Attach(new NotedProduct { ProductId = productId, UserId = userId });
        entry.State = EntityState.Deleted;
    }

    public Task<List<NotedProductDTO>> GetByProductAndUserId(Guid userId, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        return _entities
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Note)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new NotedProductDTO
            {
                Note = c.Note,
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