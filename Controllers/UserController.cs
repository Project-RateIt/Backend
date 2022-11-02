using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.Service.UserService;

namespace rateit.Controllers;

[ApiController]
[Authorize]
[Route("/api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    [AllowAnonymous]
    public Task<IActionResult> Register(string name, string surname, string email, string password) => _userService.Register(name, surname, email, password, new CancellationToken());

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password) => await _userService.Login(email, password, new CancellationToken());
    
    [HttpPost($"resetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string email) => await _userService.ResetPassword(email, new CancellationToken());
    
    [HttpPost($"setNewPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> SetNewPassword(string resetPassKey, string newPassword) => await _userService.SetNewPassword(resetPassKey, newPassword, new CancellationToken());
    
    [HttpPut($"update")]
    public async Task<IActionResult> Update(DataAccess.Entities.User user) => await _userService.UpdateUser(user, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), new CancellationToken());        
    
    [HttpPut($"changePhoto")]
    public async Task<IActionResult> ChangePhoto(string base64) => await _userService.ChangePhoto(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), base64, new CancellationToken());    
    
    [HttpDelete($"remove")]
    public async Task<IActionResult> RemoveAccount() => await _userService.RemoveAccount(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), new CancellationToken());
}