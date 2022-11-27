using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;

namespace rateit.Actions.Admin.Command;

public static class AddSubcategory
{
    public sealed record Command(Guid CategoryId, string Name) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Subcategories.AddAsync(new Subcategory
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Category = (await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken))!
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