using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Jwt;
using rateit.Services;
using rateit.Services.EmailService;

namespace rateit.Actions.User.Command;

public static class ExternalLogin
{
    public sealed record ExternalLoginCommand(string Token) : IRequest<GeneratedToken>;

    public class Handler : IRequestHandler<ExternalLoginCommand, GeneratedToken>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        private readonly IEmailService _emailService;
        private readonly IJwtAuth _jwtAuth;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService, IJwtAuth jwtAuth)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtAuth = jwtAuth;
            _pageSize = int.Parse(configuration["PageSize"]);
        }

        public async Task<GeneratedToken> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByExternalToken(request.Token, cancellationToken);
            if (user is null)
            {
                throw new BadPassword("User with this token not");
            }

            var role = user!.AccountType == AccountType.Admin ? JwtPolicies.Admin : JwtPolicies.User;
            var jwt = await _jwtAuth.GenerateJwt(user.Id, role);
            return jwt;
        }

        public sealed class Validator : AbstractValidator<ExternalLoginCommand>
        {
            public Validator()
            {

            }
        }
    }
}