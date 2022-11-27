using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Command;

public static class Rate
{
    public sealed record Command(Guid ProductId, Guid UserId, int Rate) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.RatedProducts.ExistUserProductRelationAsync(request.UserId, request.ProductId, cancellationToken))
                throw new InvalidRequestException("User already rated this product");
        
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                throw new EntityNotFoundException("Product not found");        
            if (!await _unitOfWork.Users.ExistsAsync(request.UserId, cancellationToken))
                throw new EntityNotFoundException("User not found");
        
            product.RateSum += request.Rate;
            product.RateCount += 1;

            await _unitOfWork.RatedProducts.AddAsync(new RatedProduct
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Rate = request.Rate,
                Id = Guid.NewGuid()
            }, cancellationToken);


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Rate).InclusiveBetween(1, 10);
            }
        }
    }
}