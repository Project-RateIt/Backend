using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.DataAccess.Entities;
using rateit.Jwt;
using rateit.Service.ProductService;

namespace rateit.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    
    [HttpGet($"search")]
    public async Task<IActionResult> Search(string query, int page) => await _productService.Search(query, page, new CancellationToken());
    
    [HttpPost($"rate")]
    public async Task<IActionResult> Rate(Guid productId, int rate) => await _productService.Rate(productId, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), rate, new CancellationToken());

    [HttpDelete($"unrate")]
    public async Task<IActionResult> Unrate(Guid productId) =>  await _productService.Unrate(productId, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), new CancellationToken());

    [HttpPost($"note")]
    public async Task<IActionResult> Note(Guid productId, string note) => await _productService.Note(productId, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), note, new CancellationToken());
    
    [HttpDelete($"removeNote")]
    public async Task<IActionResult> RemoveNote(Guid productId) => await _productService.RemoveNote(productId, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), new CancellationToken());
    
    [HttpPost($"viewProduct")]
    public Task<IActionResult> ViewProduct(Guid productId) => _productService.ViewProduct(productId, Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), new CancellationToken());
    
    [HttpGet($"checkProduct")]
    public async Task<IActionResult> CheckProduct(string ean) => await _productService.CheckProduct(ean, new CancellationToken());

    [HttpGet($"getRatedProduct")]
    public async Task<IActionResult> GetRatedProduct(int page) => await _productService.GetRatedProduct(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), page, new CancellationToken());

    [HttpGet($"getNotedProduct")]
    public async Task<IActionResult> GetNotedProduct(int page) => await _productService.GetNotedProduct(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), page, new CancellationToken());

    [HttpGet($"getViewedProduct")]
    public async Task<IActionResult> GetViewedProduct(int page) => await _productService.GetViewedProduct(Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "Id").Value.ToString()), page, new CancellationToken());
    
    [HttpGet($"getCategories")]
    public async Task<IActionResult> GetCategories(int page) => await _productService.GetCategories(page, new CancellationToken());
    
    [HttpGet($"getSubcategoryRanking")]
    public async Task<IActionResult> GetSubcategoryRanking(Guid subcategoryId, int page) => await _productService.GetRankingInSubcategory(subcategoryId, page, new CancellationToken());
}