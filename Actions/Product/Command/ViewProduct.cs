using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Command;

public static class ViewProduct
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
            if (!await _unitOfWork.Products.ExistsAsync(request.ProductId, cancellationToken))
                throw new EntityNotFoundException("Product not found");        
            if (!await _unitOfWork.Users.ExistsAsync(request.UserId, cancellationToken))
                throw new EntityNotFoundException("User not found");
        
            await _unitOfWork.ViewedProducts.AddAsync(new ViewedProduct
            {
                Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                ProductId = request.ProductId,
                UserId = request.UserId,
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