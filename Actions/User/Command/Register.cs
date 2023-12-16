using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Services;
using rateit.Services.EmailService;

namespace rateit.Actions.User.Command;

public static class Register
{
    public sealed record Command(string Email, string Surname, string Name, string Password) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
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

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
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
            await _emailService.SendActivateEmail(createdUser.Email, createdUser.Name, key);

            //TODO Send Email

            return Unit.Value;
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

            }
        }
    }
}