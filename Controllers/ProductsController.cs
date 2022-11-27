using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.Actions.Product.Command;
using rateit.Actions.Product.Query;
using rateit.Middlewares.Models;
using rateit.Services;

namespace rateit.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Guid _currentUserId;
    public ProductsController(IMediator mediator, IUserProvider userProvider)
    {
        _mediator = mediator;
        _currentUserId = userProvider.Id;
    }
    
    [HttpGet($"search")]
    public async Task<IActionResult> Search(string query, int page)
    {
        var result = await _mediator.Send(new Search.Query(query, page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost($"rate")]
    public async Task<IActionResult> Rate(Guid productId, int rate)
    {
        var result = await _mediator.Send(new Rate.Command(productId, _currentUserId, rate));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpDelete($"unrate")]
    public async Task<IActionResult> Unrate(Guid productId)
    {
        var result = await _mediator.Send(new Unrate.Command(productId, _currentUserId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost($"note")]
    public async Task<IActionResult> Note(Guid productId, string note)
    {
        var result = await _mediator.Send(new Note.Command(productId, _currentUserId, note));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpDelete($"removeNote")]
    public async Task<IActionResult> RemoveNote(Guid productId)
    {
        var result = await _mediator.Send(new RemoveNote.Command(productId, _currentUserId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost($"viewProduct")]
    public async Task<IActionResult> ViewProduct(Guid productId)
    {
        var result = await _mediator.Send(new ViewProduct.Command(productId, _currentUserId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"checkProduct")]
    public async Task<IActionResult> CheckProduct(string ean)
    {   
        var result = await _mediator.Send(new CheckProduct.Query(ean));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getRatedProduct")]
    public async Task<IActionResult> GetRatedProduct(int page)
    {
        var result = await _mediator.Send(new GetRatedProduct.Query(_currentUserId, page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getNotedProduct")]
    public async Task<IActionResult> GetNotedProduct(int page)
    {
        var result = await _mediator.Send(new GetNotedProduct.Query(_currentUserId, page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getViewedProduct")]
    public async Task<IActionResult> GetViewedProduct(int page)
    {
        var result = await _mediator.Send(new GetViewedProduct.Query(_currentUserId, page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getCategories")]
    public async Task<IActionResult> GetCategories(int page)
    {
        var result = await _mediator.Send(new GetCategories.Query(page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getSubcategories")]
    public async Task<IActionResult> GetSubcategories(int page, Guid categoryId)
    {
        var result = await _mediator.Send(new GetSubcategories.Query(page, categoryId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpGet($"getSubcategoryRanking")]
    public async Task<IActionResult> GetSubcategoryRanking(Guid subcategoryId, int page)
    {
        var result = await _mediator.Send(new GetRangingInSubcategory.Query(subcategoryId, page));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
}