using MediatR;
using rateit.DataAccess.Abstract;
using rateit.DTOs;

namespace rateit.Actions.Product.Query;

public static class Search
{
    public sealed record Query(string QueryString ,int Page) : IRequest<List<ProductDTO>>;

    public class Handler : IRequestHandler<Query, List<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        
        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = int.Parse(configuration["PageSize"]);
        }

        public async Task<List<ProductDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.Search(request.QueryString, request.Page, _pageSize, cancellationToken);
        }
    }
}