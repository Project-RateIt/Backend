using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DTOs;
using rateit.Exceptions;

namespace rateit.Actions.Product.Query;

public static class GetNotedProduct
{
    public sealed record Query(Guid UserId, int Page) : IRequest<List<NotedProductDTO>>;

    public class Handler : IRequestHandler<Query, List<NotedProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        
        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task<List<NotedProductDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistsAsync(request.UserId, cancellationToken))
                throw new EntityNotFoundException("User not found");
            var notedProducts = await _unitOfWork.NotedProducts.GetByProductAndUserId(request.UserId, request.Page, _pageSize, cancellationToken);

            return notedProducts;
        }
    }
}