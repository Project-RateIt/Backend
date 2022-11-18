using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.Entities;
using rateit.DTOs;

namespace rateit.Service.ProductService;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly int _pageSize; 
    
    public ProductService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _pageSize = int.Parse(configuration["PageSize"]);
    }
    public async Task<IActionResult> Rate(Guid productId, Guid userId, int rate, CancellationToken cancellationToken)
    {
        if(await _unitOfWork.RatedProducts.ExistUserProductRelationAsync(userId, productId, cancellationToken))
            return new BadRequestObjectResult("User already rated this product");
        
        var product = await _unitOfWork.Products.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return new NotFoundObjectResult("Product not found");        
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        
        product.RateSum += rate;
        product.RateCount += 1;

        await _unitOfWork.RatedProducts.AddAsync(new RatedProduct
        {
            ProductId = productId,
            UserId = userId,
            Rate = rate,
            Id = Guid.NewGuid()
        }, cancellationToken);


        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> Unrate(Guid productId, Guid userId, CancellationToken cancellationToken)
    {
        var ratedProduct = await _unitOfWork.RatedProducts.GetByUserIdAndProductId(userId, productId, cancellationToken);
        
        if (ratedProduct is null)
            return new NotFoundObjectResult("User didn't rate this product");

        var product = await _unitOfWork.Products.GetByIdAsync(productId, cancellationToken);
        if(product is null)
            return new NotFoundObjectResult("Product not found");
        
        product.RateSum -= ratedProduct.Rate;
        product.RateCount -= 1;
        
        _unitOfWork.RatedProducts.Remove(ratedProduct);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> Note(Guid productId, Guid userId, string note, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId, cancellationToken);
        
        if (product is null)
            return new NotFoundObjectResult("Product not found");        
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        
        if(await _unitOfWork.NotedProducts.ExistUserProductRelationAsync(userId, productId, cancellationToken))
            return new BadRequestObjectResult("User already noted this product");
        
        await _unitOfWork.NotedProducts.AddAsync(new NotedProduct()
        {
            ProductId = productId,
            UserId = userId,
            Note = note,
            Id = Guid.NewGuid()
        }, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
        
    }
    public async Task<IActionResult> CheckProduct(string ean, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByEan(ean, cancellationToken);
        if(product is null)
            return new NotFoundObjectResult("Product not found");
        
        return new ObjectResult(product);
    }
    public async Task<IActionResult> ViewProduct(Guid productId, Guid userId, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Products.ExistsAsync(productId, cancellationToken))
            return new NotFoundObjectResult("Product not found");        
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        
        await _unitOfWork.ViewedProducts.AddAsync(new ViewedProduct
        {
            Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
            ProductId = productId,
            UserId = userId,
            Id = Guid.NewGuid()
        }, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
        
    }
    public async Task<IActionResult> RemoveNote(Guid productId, Guid userId, CancellationToken cancellationToken)
    {

        if (!await _unitOfWork.NotedProducts.ExistUserProductRelationAsync(userId, productId, cancellationToken))
            return new NotFoundObjectResult("User didn't note this product");

        _unitOfWork.NotedProducts.RemoveByProductAndUserId(productId, userId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> Search(string query,int page, CancellationToken cancellationToken)
    {
        var x = await _unitOfWork.Products.Search(query, page, _pageSize, cancellationToken);
        return new ObjectResult(x);
    }

    public async Task<IActionResult> GetRatedProduct(Guid userId, int page, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        var ratedProducts = await  _unitOfWork.RatedProducts.GetRatedProductsAsync(userId, page, _pageSize, cancellationToken);

        return new ObjectResult(ratedProducts);
    }

    public async Task<IActionResult> GetViewedProduct(Guid userId, int page, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        var viewedProducts = await _unitOfWork.ViewedProducts.GetViewedProducts(userId, page, _pageSize, cancellationToken);
        
        return new ObjectResult(viewedProducts.Select(c => new {c.Date, c.Product}));
    }

    public async Task<IActionResult> GetCategories(int page, CancellationToken cancellationToken)
    {
        var x = await _unitOfWork.Categories.GetQueryable()
            .Skip(_pageSize * page)
            .Take(_pageSize * (page + 1))
            .Select(c => new CategoryDTO
             {
                 Id = c.Id,
                 Name = c.Name,
                 Subcategories = c.Subcategories
             }).ToListAsync(cancellationToken);
        return new ObjectResult(x); 
    }
    public async Task<IActionResult> GetNotedProduct(Guid userId, int page, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsAsync(userId, cancellationToken))
            return new NotFoundObjectResult("User not found");
        var notedProducts = await _unitOfWork.NotedProducts.GetByProductAndUserId(userId, page, _pageSize, cancellationToken);
        return new ObjectResult(notedProducts.Select(c => new {c.Product, c.Note}));
    }

    public async Task<IActionResult> GetRankingInSubcategory(Guid subcategoryId, int page, CancellationToken cancellationToken)
    {
        var ranking = await _unitOfWork.Products.GetSubcategoryRanking(subcategoryId, page, _pageSize, cancellationToken);
        return new ObjectResult(ranking);
    }

    public async Task<IActionResult> AddProduct(Product product, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Products.ExistsEanAsync(product.Ean, cancellationToken))
            return new NotFoundObjectResult("Product already exists");
        
        product.RateSum = 1;
        product.RateCount = 1;
        
        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> AddCategory(string name, CancellationToken cancellationToken)
    {
        await _unitOfWork.Categories.AddAsync(new Category
        {
            Name = name
        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new  OkResult();
    }
    public async Task<IActionResult> AddSubcategory(string name, List<string> keywords, Guid categoryId, CancellationToken cancellationToken)
    {
        await _unitOfWork.Subcategories.AddAsync(new Subcategory
        {
            Name = name,
            CategoryId = categoryId,
            Category = (await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken))!
        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        var productFromDb = await _unitOfWork.Products.GetByIdAsync(product.Id, cancellationToken);
        if(productFromDb is null)
            return new NotFoundObjectResult("Product not found");
        
        productFromDb.Name = product.Name;
        productFromDb.Ean = product.Ean;
        productFromDb.Image = product.Image;
        productFromDb.Producer = product.Producer;
        
        _unitOfWork.Products.Update(productFromDb);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();
    }
    public async Task<IActionResult> UpdateCategory(Category category, CancellationToken cancellationToken)
    {
        var categoryFromDb = await _unitOfWork.Categories.GetByIdAsync(category.Id, cancellationToken);
        if(categoryFromDb is null)
            return new NotFoundObjectResult("Category not found");
        
        categoryFromDb.Name = category.Name;
        categoryFromDb.Subcategories = category.Subcategories;

        _unitOfWork.Categories.Update(categoryFromDb);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new OkResult();
    }
    public async Task<IActionResult> UpdateSubcategory(Subcategory subcategory, CancellationToken cancellationToken)
    {
        var subcategoryFromDb = await _unitOfWork.Subcategories.GetByIdAsync(subcategory.Id, cancellationToken);
        if(subcategoryFromDb is null)
            return new NotFoundObjectResult("Subcategory not found");
        
        subcategoryFromDb.Name = subcategory.Name;
        subcategoryFromDb.CategoryId = subcategory.CategoryId;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new OkResult(); 
    }
    public async Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        _unitOfWork.Products.RemoveById(productId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult(); 
    }
    public async Task<IActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        _unitOfWork.Categories.RemoveById(categoryId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();     
    }
    public async Task<IActionResult> DeleteSubcategory(Guid subcategoryId, CancellationToken cancellationToken)
    {
        _unitOfWork.Subcategories.RemoveById(subcategoryId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new OkResult();         
    }
}

//HttpClient client = new HttpClient();
//HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
//    new Uri(
//        $"https://api.barcodelookup.com/v3/products?barcode={request.Ean}&formatted=y&key=i4liud7cw31ebt07j0i5cuphgonvbx")));
//
//dynamic content = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()) ?? new object();
//
//if (response.StatusCode == HttpStatusCode.NotFound) return StatusCode(409, "ProductNotFound");
//
//string name = content["products"][0]["title"];
//string image = content["products"][0]["images"][0];
//string producer = content["products"][0]["category"];
//string category = content["products"][0]["brand"];
//string ean = request.Ean;
//
//int id = 0;
//            
//while (true)
//{
//    Random rand = new Random();
//    id = rand.Next(10000000, 99999999);          
//    if ((!await _sqlManager.IsValueExist($"SELECT * FROM products.products WHERE id = {id};")) && (!await _sqlManager.IsValueExist($"SELECT * FROM products.orders WHERE id = {id};")))
//        break;
//}
//
//await _sqlManager.Execute(
//    $"INSERT INTO products.orders VALUES ('{ean}', {request.UserId}, '{name}', {id}, '{image}', '{producer}', '{category}')");
//
//return new ObjectResult(new Product(0.ToString(), name, 0, 0, false, false, 0, image, category, ean, producer));
//}
//catch (Exception e)
//{
//    return StatusCode(409, "ApiBardCodeError");
//    throw;
//}