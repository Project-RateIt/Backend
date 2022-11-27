using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;

namespace rateit.Actions.Admin.Command;

public static class UpdateCategory
{
    public sealed record Command(Category Category) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id, cancellationToken);
            if(categoryFromDb is null)
                throw new EntityNotFoundException("Category not found");
        
            categoryFromDb.Name = request.Category.Name;
            categoryFromDb.Subcategories = request.Category.Subcategories;

            _unitOfWork.Categories.Update(categoryFromDb);
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