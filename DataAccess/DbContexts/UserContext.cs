using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DataAccess.Repository.NoteRepository;
using rateit.DataAccess.Repository.ProductRepository;
using rateit.DataAccess.Repository.RatedProductRepository;
using rateit.DataAccess.Repository.UserRepository;
using rateit.DataAccess.Repository.ViewedProductRepository;

namespace rateit.DataAccess.DbContexts;

public class UserContext : DbContext, IUnitOfWork
{
    private DbSet<User>? _users { get; set; }
    private DbSet<Product> _products { get; set; }
    private DbSet<Category> _categories { get; set; }
    private DbSet<Subcategory> _subcategories { get; set; }
    private DbSet<NotedProduct> _notedProducts { get; set; }
    private DbSet<RatedProduct> _ratedProducts { get; set; } 
    private DbSet<ViewedProduct> _viewedProducts { get; set; }
    private DbSet<ActivateCode> _activateCodes { get; set; }

    public IUserRepository Users => new UserRepository(_users);
    public IProductRepository Products => new ProductRepository(_products);
    public IBaseRepository<Category> Categories => new BaseRepository<Category>(_categories);
    public IBaseRepository<Subcategory> Subcategories => new BaseRepository<Subcategory>(_subcategories);
    public INoteRepository NotedProducts => new NoteRepository(_notedProducts);
    public IRatedProductRepository RatedProducts => new RatedProductRepository(_ratedProducts);
    public IViewedProductRepository ViewedProducts => new ViewedProductRepository(_viewedProducts);
    public IBaseRepository<ActivateCode> ActivateCodes => new BaseRepository<ActivateCode>(_activateCodes); 

    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    public UserContext() : base() { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}