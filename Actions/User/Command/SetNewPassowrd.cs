using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.User.Command;

public static class SetNewPassword
{
    public sealed record Command(string ResetPassKey, string NewPassword) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByResetPasswordKey(request.ResetPassKey, cancellationToken);

            if(user is null)
            {
                throw new EntityNotFoundException("Reset password key not exist");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken:cancellationToken);
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