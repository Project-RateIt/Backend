using rateit.Helpers;

namespace rateit.Products;

public class FollowRequestModel : Request
{
    public FollowRequestModel(int productId, int userId, string token) : base(token)
    {
        ProductId = productId;
        UserId = userId;
    }    
    
    public int ProductId { get; }
    public int UserId { get; }
}