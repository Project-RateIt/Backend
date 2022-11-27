using MediatR;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Abstract;
using rateit.DTOs;
using rateit.Services;

namespace rateit.Actions.Product.Query;

public static class GetCategories
{
    public sealed record Query(int Page) : IRequest<List<CategoryDTO>>;

    public class Handler : IRequestHandler<Query, List<CategoryDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;
        
        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task<List<CategoryDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var x = await _unitOfWork.Categories.GetQueryable()
                .Skip(_pageSize * request.Page)
                .Take(_pageSize * (request.Page + 1))
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subcategories = c.Subcategories
                }).ToListAsync(cancellationToken);

            return x;
        }
    }
}