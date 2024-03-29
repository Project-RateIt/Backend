using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.Actions.User.Command;
using rateit.Actions.User.Query;
using rateit.Middlewares.Models;
using rateit.Services;

namespace rateit.Controllers;

[ApiController]
[Produces("application/json")]
[Route("/api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Guid _currentUserId;
    public UserController(IMediator mediator, IUserProvider userProvider)
    {
        _mediator = mediator;
        _currentUserId = userProvider.Id;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(string name, string surname, string email, string password)
    {
        var result = await _mediator.Send(new Register.Command(email, surname, name, password));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _mediator.Send(new Login.Query(email, password));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    // Refresh token endpoint
    // [HttpPost("refresh")]
    // [AllowAnonymous]
    // public async Task<IActionResult> Refresh(Guid id) => await _userService.RefreshToken(id, new CancellationToken());

    [HttpGet("activate")]
    public async Task<IActionResult> Activate(string email, string code)
    {
        var result = await _mediator.Send(new ActivateUser.Command(email, code));
        return RedirectPermanent("https://www.google.com");
    }

    [HttpPost($"resetPassword")]
    public async Task<IActionResult> ResetPassword(string email)
    {
        var result = await _mediator.Send(new ResetPassword.Command(email));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
    

    [HttpPost($"setNewPassword")]
    public async Task<IActionResult> SetNewPassword([FromForm]string resetPassKey, [FromForm]string newPassword)
    {
        var result = await _mediator.Send(new SetNewPassword.Command(resetPassKey, newPassword));
        return RedirectPermanent("https://www.google.com");
    }
    
    [Authorize]
    [HttpPut($"update")]
    public async Task<IActionResult> Update(DataAccess.Entities.User user)
    {
        var result = await _mediator.Send(new UpdateUser.Command(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), user));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [Authorize]
    [HttpPut($"changePhoto")]
    public async Task<IActionResult> ChangePhoto(string base64)
    {
        var result = await _mediator.Send(new ChangePhoto.Command(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), base64));
        return new ObjectResult(ApiResponse.Success(200, result));
        
    }

    [Authorize]
    [HttpDelete($"remove")]
    public async Task<IActionResult> RemoveAccount()
    {
        var result = await _mediator.Send(new RemoveAccount.Command(_currentUserId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
    
    [HttpPost("externalLogin")]
    public async Task<IActionResult> ExternalLogin(ExternalLogin.ExternalLoginCommand externalLoginExternalLoginCommand)
    {
        var result = await _mediator.Send(externalLoginExternalLoginCommand);
        return new ObjectResult(ApiResponse.Success(200, result));
    }    
    
    [HttpPost("externalRegister")]
    public async Task<IActionResult> ExternalRegister(ExternalRegister.ExternalRegisterCommand externalRegisterExternalRegisterCommand)
    {
        var result = await _mediator.Send(externalRegisterExternalRegisterCommand);
        return new ObjectResult(ApiResponse.Success(200, result));
    }
}