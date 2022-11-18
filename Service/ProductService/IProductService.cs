using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Entities;

namespace rateit.Service.ProductService;

public interface IProductService
{
    Task<IActionResult> Rate(Guid productId, Guid userId, int rate, CancellationToken cancellationToken);
    Task<IActionResult> Unrate(Guid productId, Guid userId, CancellationToken cancellationToken);
    Task<IActionResult> Note(Guid productId, Guid userId, string note, CancellationToken cancellationToken);
    Task<IActionResult> CheckProduct(string ean, CancellationToken cancellationToken);
    Task<IActionResult> ViewProduct(Guid productId, Guid userId, CancellationToken cancellationToken);
    Task<IActionResult> RemoveNote(Guid productId, Guid userId, CancellationToken cancellationToken);
    Task<IActionResult> Search(string query, int page, CancellationToken cancellationToken);
    Task<IActionResult> GetRatedProduct(Guid userId, int page, CancellationToken cancellationToken);
    Task<IActionResult> GetViewedProduct(Guid userId, int page, CancellationToken cancellationToken);
    Task<IActionResult> GetCategories(int page, CancellationToken cancellationToken);
    Task<IActionResult> GetRankingInSubcategory(Guid subcategoryId, int page, CancellationToken cancellationToken);
    Task<IActionResult> GetNotedProduct(Guid userId, int page, CancellationToken cancellationToken);
    Task<IActionResult> AddProduct(Product product, CancellationToken cancellationToken);
    Task<IActionResult> AddCategory(string name, CancellationToken cancellationToken);
    Task<IActionResult> AddSubcategory(string name, List<string> keywords, Guid categoryId, CancellationToken cancellationToken);
    Task<IActionResult> UpdateProduct(Product product, CancellationToken cancellationToken);
    Task<IActionResult> UpdateCategory(Category category, CancellationToken cancellationToken);
    Task<IActionResult> UpdateSubcategory(Subcategory subcategory, CancellationToken cancellationToken);
    Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken);
    Task<IActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken);
    Task<IActionResult> DeleteSubcategory(Guid subcategoryId, CancellationToken cancellationToken);
    
}