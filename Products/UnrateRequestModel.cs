using rateit.Helpers;

namespace rateit.Products;

public class UnrateRequestModel : Request
{
    public UnrateRequestModel(string token, string userId, string productId) : base(token)
    {
        UserId = userId;
        ProductId = productId;
    }

    public string UserId { get;}
    public string ProductId { get; }
}