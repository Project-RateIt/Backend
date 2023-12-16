using MediatR;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Jwt;
using rateit.Services;

namespace rateit.Actions.User.Query;

public static class Login
{
    public sealed record Query(string Email, string Password) : IRequest<GeneratedToken>;

    public class Handler : IRequestHandler<Query, GeneratedToken>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtAuth _jwtAuth;
        private readonly int _pageSize;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration, IJwtAuth jwtAuth)
        {
            _unitOfWork = unitOfWork;
            _jwtAuth = jwtAuth;
            _pageSize = int.Parse(configuration["PageSize"]);
        }

        public async Task<GeneratedToken> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistEmailAsync(request.Email, cancellationToken))
            {
                throw new InvalidRequestException("Email not exist");
            }
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, _pageSize, cancellationToken);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new InvalidRequestException("Password is incorrect");
            }        
            if (!user.IsActive)
            {
                throw new InvalidRequestException("User is not active");
            }
        
        
            string role;
            if (user.AccountType == AccountType.Admin)
                role = JwtPolicies.Admin;
            else
                role = JwtPolicies.User;
        
            GeneratedToken jwt = await _jwtAuth.GenerateJwt(user.Id, role);
            return jwt;
        }
    }
}