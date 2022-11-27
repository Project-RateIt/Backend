using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.Actions.User.Command;
using rateit.Actions.User.Query;
using rateit.Middlewares.Models;

namespace rateit.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
[Route("/api/user")]
public class UserController : ControllerBase
{
    private IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string name, string surname, string email, string password)
    {
        var result = await _mediator.Send(new Register.Command(email, surname, name, password));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _mediator.Send(new Login.Query(email, password));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    // Refresh token endpoint
    // [HttpPost("refresh")]
    // [AllowAnonymous]
    // public async Task<IActionResult> Refresh(Guid id) => await _userService.RefreshToken(id, new CancellationToken());

    [HttpPut("activate")]
    [AllowAnonymous]
    public async Task<IActionResult> Activate(string email, string code)
    {
        var result = await _mediator.Send(new ActivateUser.Command(email, code));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost($"resetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string email)
    {
        var result = await _mediator.Send(new ResetPassword.Command(email));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
    

    [HttpPost($"setNewPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> SetNewPassword(string resetPassKey, string newPassword)
    {
        var result = await _mediator.Send(new SetNewPassword.Command(resetPassKey, newPassword));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPut($"update")]
    public async Task<IActionResult> Update(DataAccess.Entities.User user)
    {
        var result = await _mediator.Send(new UpdateUser.Command(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), user));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPut($"changePhoto")]
    public async Task<IActionResult> ChangePhoto(string base64)
    {
        var result = await _mediator.Send(new ChangePhoto.Command(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), base64));
        return new ObjectResult(ApiResponse.Success(200, result));
        
    }

    [HttpDelete($"remove")]
    public async Task<IActionResult> RemoveAccount()
    {
        var result = await _mediator.Send(new RemoveAccount.Command(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString())));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
}