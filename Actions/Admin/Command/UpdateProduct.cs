using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Admin.Command;

public static class UpdateProduct
{
    public sealed record Command(DataAccess.Entities.Product Product) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var productFromDb = await _unitOfWork.Products.GetByIdAsync(request.Product.Id, cancellationToken);
            if(productFromDb is null)
                throw new EntityNotFoundException("Product not found");
        
            productFromDb.Name = request.Product.Name;
            productFromDb.Ean = request.Product.Ean;
            productFromDb.Image = request.Product.Image;
            productFromDb.Producer = request.Product.Producer;
        
            _unitOfWork.Products.Update(productFromDb);
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