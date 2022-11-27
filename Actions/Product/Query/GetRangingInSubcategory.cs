using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DTOs;
using rateit.Services;

namespace rateit.Actions.Product.Query;

public static class GetRangingInSubcategory
{
    public sealed record Query(Guid SubcategoryId, int Page) : IRequest<List<ProductDTO>>;

    public class Handler : IRequestHandler<Query, List<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task<List<ProductDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ranking = await _unitOfWork.Products.GetSubcategoryRanking(request.SubcategoryId, request.Page, _pageSize, cancellationToken);
            return ranking;
        }
    }
}