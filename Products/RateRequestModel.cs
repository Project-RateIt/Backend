using rateit.Helpers;

namespace rateit.Products;

public class RateRequestModel : Request
{
    public RateRequestModel(int productId, int rate, string token, int userId) : base(token)
    {
        ProductId = productId;
        Rate = rate;
        UserId = userId;
    }

    public int UserId { get; }
    public int ProductId { get; }
    public int Rate { get; }
}