using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rateit.Actions.Admin.Command;
using rateit.DataAccess.Entities;
using rateit.Jwt;
using rateit.Middlewares.Models;
using rateit.Service.ProductService;

namespace rateit.Controllers;

[ApiController]
[Authorize(JwtPolicies.Admin)]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("addProduct")]
    public async Task<IActionResult> AddProduct(Product product)
    {
        var result = await _mediator.Send(new AddProduct.Command(product));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost("addCategory")]
    public async Task<IActionResult> AddCategory(string name)
    {
        var result = await _mediator.Send(new AddCategory.Command(name));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPost("addSubcategory")]
    public async Task<IActionResult> AddSubcategory(string name, Guid categoryId)
    {
        var result = await _mediator.Send(new AddSubcategory.Command(categoryId, name));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPut("updateProduct")]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        var result = await _mediator.Send(new UpdateProduct.Command(product));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPut("updateCategory")]
    public async Task<IActionResult> UpdateCategory(Category category)
    {
        var result = await _mediator.Send(new UpdateCategory.Command(category));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpPut("updateSubcategory")]
    public async Task<IActionResult> UpdateSubcategory(Subcategory subcategory)
    {
        var result = await _mediator.Send(new UpdateSubcategory.Command(subcategory));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpDelete("deleteProduct")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        var result = await _mediator.Send(new DeleteProduct.Command(productId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpDelete("deleteCategory")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        var result = await _mediator.Send(new DeleteCategory.Command(categoryId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }

    [HttpDelete("deleteSubcategory")]
    public async Task<IActionResult> DeleteSubcategory(Guid subcategoryId)
    {
        var result = await _mediator.Send(new DeleteSubcategory.Command(subcategoryId));
        return new ObjectResult(ApiResponse.Success(200, result));
    }
}