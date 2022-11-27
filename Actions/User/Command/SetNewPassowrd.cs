using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Helpers;

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