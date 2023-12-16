using rateit.DataAccess.Entities;
using rateit.DataAccess.Repository.BaseRepository;
using rateit.DTOs;

namespace rateit.DataAccess.Repository.UserRepository;

public interface IUserRepository : IBaseRepository<Entities.User>
{
    public Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<UserDTO> GetByEmailAsync(string email, int PageSize, CancellationToken cancellationToken = default);
    public Task<Entities.User> GetByResetPasswordKey(string key, CancellationToken cancellationToken = default);
    Task<User?> GetByExternalToken(string token, CancellationToken cancellationToken);
}