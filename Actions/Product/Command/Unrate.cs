using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Command;

public static class Unrate
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
            var ratedProduct = await _unitOfWork.RatedProducts.GetByUserIdAndProductId(request.UserId, request.ProductId, cancellationToken);
        
            if (ratedProduct is null)
                throw new EntityNotFoundException("User didn't rate this product");

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
            if(product is null)
                throw new EntityNotFoundException("Product not found");
        
            product.RateSum -= ratedProduct.Rate;
            product.RateCount -= 1;
        
            _unitOfWork.RatedProducts.Remove(ratedProduct);
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