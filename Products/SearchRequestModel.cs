using rateit.Helpers;

namespace rateit.Products;

public class SearchRequestModel : Request
{
    public SearchRequestModel(int page, string query , int userId, string token) : base(token)
    {
        Page = page;
        Query = query;
        UserId = userId;
    }

    public string Query{ get; }
    public int Page { get; }
    public int UserId { get; }
}