using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.User.Command;

public static class UpdateUser
{
    public sealed record Command(Guid IdFromJwt, DataAccess.Entities.User User) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userFromDb = await _unitOfWork.Users.GetByIdAsync(request.IdFromJwt, cancellationToken);
            if (userFromDb is null)
            {
                throw new InvalidRequestException("User not exist");
            }

            userFromDb.Id = request.IdFromJwt;
            userFromDb.Name = request.User.Name ?? userFromDb.Name;
            userFromDb.Surname = request.User.Surname ?? userFromDb.Surname;;  
            userFromDb.Email = request.User.Email ?? userFromDb.Email;
        
            _unitOfWork.Users.Update(userFromDb);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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