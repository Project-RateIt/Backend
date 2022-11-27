using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DTOs;
using rateit.Exceptions;
using rateit.Services;

namespace rateit.Actions.Product.Query;

public static class GetViewedProduct
{
    public sealed record Query(Guid UserId, int Page) : IRequest<List<ViewedProductDTO>>;

    public class Handler : IRequestHandler<Query, List<ViewedProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        
        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task<List<ViewedProductDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistsAsync(request.UserId, cancellationToken))
                throw new EntityNotFoundException("User not found");
            var viewedProducts = await _unitOfWork.ViewedProducts.GetViewedProducts(request.UserId, request.Page, _pageSize, cancellationToken);
            return viewedProducts;
        }
    }
}