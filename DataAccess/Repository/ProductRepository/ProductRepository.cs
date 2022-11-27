using System.Linq;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;
using Product = rateit.DataAccess.Entities.Product;

namespace rateit.DataAccess.Repository.ProductRepository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DbSet<Product>? entities) : base(entities)
    {
    }

    public Task<List<ProductDTO>> Search(string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var x = _entities
            .Where(c=> c.Name!.ToLower().Contains(query.ToLower()))
            .OrderBy(c=> c.RateSum/c.RateCount).ThenBy(c=> c.Name)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new ProductDTO {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                Ean = c.Ean,
                Producer = c.Producer,
                RateCount = c.RateCount,
                RateSum = c.RateSum,
                Sponsor = c.Sponsor,
                Subcategory = c.Subcategory,
                SubcategoryId = c.SubcategoryId,
                
            });

        return x.ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByEan(string ean, CancellationToken cancellationToken)
    {
        return _entities.FirstOrDefaultAsync(c => c.Ean == ean, cancellationToken);
    }

    public Task<List<ProductDTO>> GetSubcategoryRanking(Guid subcategoryId, int page, int pageSize, CancellationToken cancellationToken)
    {
        return _entities.Where(c => c.SubcategoryId == subcategoryId)
            .OrderBy(c => c.RateSum / c.RateCount)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(c => new ProductDTO
            {
                Ean = c.Ean,
                Name = c.Name,
                Image = c.Image,
                RateCount = c.RateCount,
                RateSum = c.RateSum,
                Subcategory = c.Subcategory,
                SubcategoryId = c.SubcategoryId,
                Sponsor = c.Sponsor,
                Producer = c.Producer,
                Id = c.Id,
                NotedProducts = c.NotedProducts,
                RatedProducts = c.RatedProducts,
                ViewedProducts = c.ViewedProducts
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsEanAsync(string ean, CancellationToken cancellationToken)
    {
        return _entities.AnyAsync(c => c.Ean == ean, cancellationToken);
    }
}