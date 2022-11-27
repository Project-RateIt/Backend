using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;

namespace rateit.Actions.Admin.Command;

public static class DeleteSubcategory
{
    public sealed record Command(Guid SubcategoryId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Products.ExistsAsync(request.SubcategoryId, cancellationToken))
            {
                throw new EntityNotFoundException("Subcategory not found");
            }
            
            _unitOfWork.Subcategories.RemoveById(request.SubcategoryId);
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