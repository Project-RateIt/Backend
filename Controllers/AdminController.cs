using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Entities;
using rateit.Jwt;
using rateit.Service.ProductService;

namespace rateit.Controllers;

[ApiController]
[Authorize(JwtPolicies.Admin)]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("addProduct")]
    public async Task<IActionResult> AddProduct(Product product) => await _productService.AddProduct(product, new CancellationToken());
    
    [HttpPost("addCategory")]
    public async Task<IActionResult> AddCategory(string name) => await _productService.AddCategory(name, new CancellationToken());
    
    [HttpPost("addSubcategory")]
    public async Task<IActionResult> AddSubcategory(string name, List<string> keywords, Guid categoryId) => await _productService.AddSubcategory(name, keywords, categoryId, new CancellationToken());
    
    [HttpPut("updateProduct")]
    public async Task<IActionResult> UpdateProduct(Product product) => await _productService.UpdateProduct(product, new CancellationToken());
    
    [HttpPut("updateCategory")]
    public async Task<IActionResult> UpdateCategory(Category category) => await _productService.UpdateCategory(category, new CancellationToken());
    
    [HttpPut("updateSubcategory")]
    public async Task<IActionResult> UpdateSubcategory(Subcategory subcategory) => await _productService.UpdateSubcategory(subcategory, new CancellationToken());
    
    [HttpDelete("deleteProduct")]
    public async Task<IActionResult> DeleteProduct(Guid productId) => await _productService.DeleteProduct(productId, new CancellationToken());
    
    [HttpDelete("deleteCategory")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId) => await _productService.DeleteCategory(categoryId, new CancellationToken());

    [HttpDelete("deleteSubcategory")]
    public async Task<IActionResult> DeleteSubcategory(Guid subcategoryId) => await _productService.DeleteSubcategory(subcategoryId, new CancellationToken());
}