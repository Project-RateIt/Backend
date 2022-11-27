using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.User.Command;

public static class ChangePhoto
{
    public sealed record Command(Guid Id, string Photo) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string? _pathToImages;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pathToImages = configuration["PathToImages"];
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
            {
                throw new InvalidRequestException("User not exist");
            }
        
            user.HaveAvatar = true;
            try
            {
                await File.WriteAllBytesAsync(_pathToImages + $@"/{request.Id}", Convert.FromBase64String(request.Photo), cancellationToken);
            }
            catch (Exception e)
            {
                await File.WriteAllBytesAsync($@"/{request.Id}", Convert.FromBase64String(request.Photo), cancellationToken);
            }
        
            _unitOfWork.Users.Update(user);
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