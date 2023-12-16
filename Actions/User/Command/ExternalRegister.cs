using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Services;
using rateit.Services.EmailService;

namespace rateit.Actions.User.Command;

public static class ExternalRegister
{
    public sealed record ExternalRegisterCommand(string Email, string Surname, string Name, string Token) : IRequest<Unit>;

    public class Handler : IRequestHandler<ExternalRegisterCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        private readonly IEmailService _emailService;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _pageSize = int.Parse(configuration["PageSize"]);
        }

        public async Task<Unit> Handle(ExternalRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, _pageSize, cancellationToken);
            if (user != null)
            {
                throw new InvalidRequestException("User with this email already exists");
            }
            DataAccess.Entities.User createdUser = new DataAccess.Entities.User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PasswordHash = request.Token,
                AccountType = AccountType.User,
                HaveAvatar = false,
                IsActive = true,
                IsExternal = true,
            };
        
            
            await _unitOfWork.Users.AddAsync(createdUser, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public sealed class Validator : AbstractValidator<ExternalRegisterCommand>
        {
            public Validator()
            {

            }
        }
    }
}