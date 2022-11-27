using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;

namespace rateit.Actions.Admin.Command;

public static class UpdateSubcategory
{
    public sealed record Command(Subcategory Subcategory) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var subcategoryFromDb = await _unitOfWork.Subcategories.GetByIdAsync(request.Subcategory.Id, cancellationToken);
            if(subcategoryFromDb is null)
                throw new EntityNotFoundException("Subcategory not found");
        
            subcategoryFromDb.Name = request.Subcategory.Name;
            subcategoryFromDb.CategoryId = request.Subcategory.CategoryId;
        
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