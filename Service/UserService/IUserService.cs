using Microsoft.AspNetCore.Mvc;

namespace rateit.Service.UserService;

public interface IUserService
{
    Task<IActionResult> Register(string name, string surname, string email, string password, CancellationToken cancellationToken);
    Task<IActionResult> Login(string email, string password, CancellationToken cancellationToken);
    Task<IActionResult> RefreshToken(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateUser(DataAccess.Entities.User user, Guid idFromJwt, CancellationToken cancellationToken);
    Task<IActionResult> RemoveAccount(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> ChangePhoto(Guid id, string base64, CancellationToken cancellationToken);
    Task<IActionResult> ResetPassword(string email, CancellationToken cancellationToken);
    Task<IActionResult> SetNewPassword(string resetPassKey, string newPassword, CancellationToken cancellationToken);
    Task<IActionResult> ActivateUser(string activateCode, string email, CancellationToken cancellationToken);
    Task<IActionResult> ResendActivateCode(string activateCode, string email, CancellationToken cancellationToken);
}