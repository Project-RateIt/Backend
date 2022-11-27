using MediatR;
using rateit.DataAccess.Abstract;
using rateit.Exceptions;
using rateit.Helpers;

namespace rateit.Actions.Product.Query;

public static class CheckProduct
{
    public sealed record Query(string Ean) : IRequest<DataAccess.Entities.Product?>;

    public class Handler : IRequestHandler<Query, DataAccess.Entities.Product?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DataAccess.Entities.Product?> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByEan(request.Ean, cancellationToken);
            if(product is null)
                throw new EntityNotFoundException("Product not found");

            return product;
        }
    }
}