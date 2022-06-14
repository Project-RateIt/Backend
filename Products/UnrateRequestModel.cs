using rateit.Helpers;

namespace rateit.Products;

public class UnrateRequestModel : Request
{
    public UnrateRequestModel(string token, int userId, string productId) : base(token)
    {
        UserId = userId;
        ProductId = productId;
    }

    public int UserId { get;}
    public string ProductId { get; }
}