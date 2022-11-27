using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;

namespace rateit.Actions.User.Command;

public static class ActivateUser
{
    public sealed record Command(string Email, string ActivationCode) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, 0, cancellationToken);
            if (user is null)
            {
                throw new InvalidRequestException("User is not exist");
            }

            if (user.IsActive)
            {
                throw new InvalidRequestException("User is already active");
            }

            if (user.ActivateCode.Code == request.ActivationCode)
            {
                _unitOfWork.Users.Update(new DataAccess.Entities.User
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
            }
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