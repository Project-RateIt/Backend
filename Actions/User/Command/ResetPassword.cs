using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.User.Command;

public static class ResetPassword
{
    public sealed record Command(string Email) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistEmailAsync(request.Email, cancellationToken))
            {
                throw new EntityNotFoundException("Email not exist");
            }
        
            Random random = new Random();
        
            string chars = "1234567890qwertyuiopasdfgthjklQWERTYUIOPASDFGHJKLZXCVBNMzxcvbnm";
            string key = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            //TODO: Send email with key

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