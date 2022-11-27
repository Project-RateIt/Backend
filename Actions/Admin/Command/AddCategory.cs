using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Services;

namespace rateit.Actions.Admin.Command;

public static class AddCategory
{
    public sealed record Command(string Name) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {        await _unitOfWork.Categories.AddAsync(new Category {
                Name = request.Name
            }, cancellationToken);
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