using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Command;

public static class Note
{
    public sealed record Command(Guid ProductId, Guid UserId, string Note) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
        
            if (product is null)
                throw new EntityNotFoundException("Product not found");        
            if (!await _unitOfWork.Users.ExistsAsync(request.UserId, cancellationToken))
                throw new EntityNotFoundException("User not found");
        
            if(await _unitOfWork.NotedProducts.ExistUserProductRelationAsync(request.UserId, request.ProductId, cancellationToken))
                throw new InvalidRequestException("User already noted this product");
        
            await _unitOfWork.NotedProducts.AddAsync(new NotedProduct()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Note = request.Note,
                Id = Guid.NewGuid()
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