using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.DTOs;

namespace rateit.Actions.Product.Query;

public static class GetSubcategories
{
    public sealed record Query(int Page, Guid CategoryId) : IRequest<List<SubcategoryDTO>>;

    public class Handler : IRequestHandler<Query, List<SubcategoryDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _pageSize;

        public Handler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task<List<SubcategoryDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var x = await _unitOfWork.Subcategories
                .GetQueryable()
                .Where(c => c.CategoryId == request.CategoryId)
                .Skip(_pageSize * request.Page)
                .Take(_pageSize * (request.Page + 1))
                .Select(c => new SubcategoryDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products
                }).ToListAsync(cancellationToken);

            return x;
        }
    }
}