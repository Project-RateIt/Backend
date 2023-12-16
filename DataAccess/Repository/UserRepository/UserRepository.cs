using System.Linq;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.UserRepository;

public class UserRepository : BaseRepository<Entities.User>, IUserRepository
{
    public UserRepository(DbSet<Entities.User>? entities) : base(entities)
    {
    }
    
    public Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken) => _entities.AnyAsync(user => user.Email == email, cancellationToken)!;
    public Task<UserDTO> GetByEmailAsync(string email, int pageSize, CancellationToken cancellationToken = default)
    {
        return _entities.Where(user => user.Email == email).Select(c => new UserDTO
        {
            Email = c.Email,
            Id = c.Id,
            Name = c.Name,
            Surname = c.Surname,
            AccountType = c.AccountType,
            AddedProduct = -+c.AddedProduct,
            HaveAvatar = c.HaveAvatar,
            IsActive = c.IsActive,
            PasswordHash = c.PasswordHash,
            ActivateCode = c.ActivateCode,
            NotedProducts = c.NotedProducts.Select(d => new NotedProductDTO
            {
                Note = d.Note,
                Product = d.Product,
                ProductId = d.ProductId,
            }).Take(pageSize).ToList(),
            RatedProducts = c.RatedProducts.Select(d => new RatedProductDTO
            {
                Product = d.Product,
                ProductId = d.ProductId,
                Rate = d.Rate,
            }).Take(pageSize).ToList(),
            ViewedProducts = c.ViewedProducts.Select(d => new ViewedProductDTO
            {
                Product = d.Product,
                ProductId = d.ProductId,
                Date = d.Date,
            }).Take(pageSize).ToList(),
        }).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Entities.User> GetByResetPasswordKey(string key, CancellationToken cancellationToken = default) => _entities.FirstOrDefaultAsync(c => c.ResetPassKey == key, cancellationToken)!;
    public Task<User?> GetByExternalToken(string token, CancellationToken cancellationToken) => _entities.FirstOrDefaultAsync(c => c.PasswordHash == token && c.IsExternal, cancellationToken)!;
}