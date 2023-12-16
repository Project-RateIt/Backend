using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;
using rateit.Services.EmailService;

namespace rateit.Actions.User.Command;

public static class ResetPassword
{
    public sealed record Command(string Email) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, 0, cancellationToken);
            var userFromDb = await _unitOfWork.Users.GetByIdAsync(user.Id, cancellationToken);

            if (userFromDb is null)
            {
                throw new EntityNotFoundException("Email not exist");
            }
        
            Random random = new Random();
        
            string chars = "1234567890qwertyuiopasdfgthjklQWERTYUIOPASDFGHJKLZXCVBNMzxcvbnm";
            string key = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            userFromDb.ResetPassKey = key;
            _unitOfWork.Users.Update(userFromDb);
            await _unitOfWork.SaveChangesAsync(cancellationToken:cancellationToken);
            if (user.Name != null) await _emailService.SendResetPasswordEmail(request.Email, user.Name, key);

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