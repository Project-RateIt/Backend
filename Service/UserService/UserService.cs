using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.DTOs;
using rateit.Jwt;

namespace rateit.Service.UserService;
public class UserService : IUserService
{
    private readonly IJwtAuth _jwtAuth;
    private readonly IUnitOfWork _unitOfWork;
    private readonly int _pageSize;
    private readonly string _pathToImages;
    
    public UserService(IUnitOfWork unitOfWork, IJwtAuth jwtAuth, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtAuth = jwtAuth;
        _pathToImages = configuration.GetValue<string>("PathToImages");
        _pageSize = configuration.GetValue<int>("PageSize");
    }

    public async Task<IActionResult> Register(string name, string surname, string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email, _pageSize, cancellationToken);
        if (user != null)
        {
            return new BadRequestObjectResult("User with this email already exists");
        }

        User createdUser = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            AccountType = AccountType.User,
            HaveAvatar = false,
            IsActive = false
        };
        
        await _unitOfWork.Users.AddAsync(createdUser, cancellationToken);

        Random random = new Random();
        
        string chars = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
        string key = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        await _unitOfWork.ActivateCodes.AddAsync(new ActivateCode
        {
            Code = key,
            UserId = createdUser.Id,
            Id = Guid.NewGuid(),
            User = createdUser
        }, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        //TODO Send Email

        return new OkObjectResult("User successfully registered");
    }
    public async Task<IActionResult> Login(string email, string password, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistEmailAsync(email, cancellationToken))
        {
            return new BadRequestObjectResult("Email not exist");
        }
        var user = await _unitOfWork.Users.GetByEmailAsync(email, _pageSize, cancellationToken);
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new BadRequestObjectResult("Password is incorrect");
        }        
        if (!user.IsActive)
        {
            return new BadRequestObjectResult("User is not active");
        }
        
        
        string role;
        if (user.AccountType == AccountType.Admin)
            role = JwtPolicies.Admin;
        else
            role = JwtPolicies.User;
        
        string jwt = await _jwtAuth.GenerateJwt(user.Id, role);
        return new OkObjectResult(new {
            jwt,
            user.Id, 
            user.Name, 
            user.Surname,
            user.Email, 
            user.AccountType, 
            user.HaveAvatar, 
            user.AddedProduct,
            user.NotedProducts,
            user.RatedProducts,
            user.ViewedProducts
        });
    }

    public async Task<IActionResult> RefreshToken(Guid id, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return new BadRequestObjectResult("User not found");
        }
        string role;
        if (user.AccountType == AccountType.Admin)
            role = JwtPolicies.Admin;
        else
            role = JwtPolicies.User;
        
        string jwt = await _jwtAuth.GenerateJwt(user.Id, role);
        return new ObjectResult(jwt);
    }

    public async Task<IActionResult> UpdateUser(User user, Guid idFromJwt, CancellationToken cancellationToken)
    {
        var userFromDb = await _unitOfWork.Users.GetByIdAsync(idFromJwt, cancellationToken);
        if (userFromDb is null)
        {
            return new BadRequestObjectResult("User not exist");
        }

        userFromDb.Id = idFromJwt;
        userFromDb.Name = user.Name ?? userFromDb.Name;
        userFromDb.Surname = user.Surname ?? userFromDb.Surname;;  
        userFromDb.Email = user.Email ?? userFromDb.Email;
        
        _unitOfWork.Users.Update(userFromDb);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> ChangePhoto(Guid id, string base64, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return new BadRequestObjectResult("User not exist");
        }
        
        user.HaveAvatar = true;
        try
        {
            await File.WriteAllTextAsync(_pathToImages + id, base64, cancellationToken);
        }
        catch (Exception e)
        {
            await File.WriteAllTextAsync($@"/{id}", base64, cancellationToken);
        }
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
        
    }
    public async Task<IActionResult> RemoveAccount(Guid id, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsAsync(id, cancellationToken))
        {
            return new BadRequestObjectResult("User not exist");
        }

        _unitOfWork.Users.RemoveById(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> ResetPassword(string email, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistEmailAsync(email, cancellationToken))
        {
            return new BadRequestObjectResult("Email not exist");
        }
        
        Random random = new Random();
        
        string chars = "1234567890qwertyuiopasdfgthjklQWERTYUIOPASDFGHJKLZXCVBNMzxcvbnm";
        string key = new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        //Send email with key

        return new OkResult();
    }
    public async Task<IActionResult> SetNewPassword(string resetPassKey, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByResetPasswordKey(resetPassKey, cancellationToken);
        if(user is null)
        {
            return new BadRequestObjectResult("Reset password key not exist");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        _unitOfWork.Users.Update(user);
        return new OkObjectResult(_unitOfWork.SaveChangesAsync(cancellationToken));
    }
    
    public async Task<IActionResult> ActivateUser(string activateCode, string email, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email, 0,  cancellationToken);
        if (user is null)
        {
            return new BadRequestObjectResult("User is not exist");
        }

        if (user.IsActive)
        {
            return new BadRequestObjectResult("User is already active");
        }

        if(user.ActivateCode.Code == activateCode)
        {
            _unitOfWork.Users.Update(new User
            {
                Id = user.Id,
                IsActive = true,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                PasswordHash = user.PasswordHash,
                AccountType = user.AccountType,
                HaveAvatar = user.HaveAvatar,
                
            });
            
            _unitOfWork.ActivateCodes.RemoveById(user.ActivateCode.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new OkResult();
        }

        return new BadRequestObjectResult("Activate code is incorrect");
    }

    public Task<IActionResult> ResendActivateCode(string activateCode, string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}