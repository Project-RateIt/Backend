using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Command;

public static class RemoveNote
{
    public sealed record Command(Guid ProductId, Guid UserId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.NotedProducts.ExistUserProductRelationAsync(request.UserId, request.ProductId, cancellationToken))
                throw new EntityNotFoundException("User didn't note this product");

            _unitOfWork.NotedProducts.RemoveByProductAndUserId(request.ProductId, request.UserId);
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