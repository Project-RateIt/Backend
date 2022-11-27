using FluentValidation;
using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Admin.Command;

public static class AddProduct
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
            if (await _unitOfWork.Products.ExistsEanAsync(request.Product.Ean!, cancellationToken))
                throw new InvalidRequestException("Product already exists");
        
            request.Product.RateSum = 1;
            request.Product.RateCount = 1;
        
            await _unitOfWork.Products.AddAsync(request.Product, cancellationToken);
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