using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DataAccess.Repository.NoteRepository;
using rateit.DataAccess.Repository.ProductRepository;
using rateit.DataAccess.Repository.RatedProductRepository;
using rateit.DataAccess.Repository.UserRepository;
using rateit.DataAccess.Repository.ViewedProductRepository;

namespace rateit.Services;

public interface IUnitOfWork
{
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Subcategory> Subcategories { get; }
        INoteRepository NotedProducts { get; }
        IRatedProductRepository RatedProducts { get; }
        IViewedProductRepository ViewedProducts { get; }
        IBaseRepository<ActivateCode> ActivateCodes { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}