using rateit.Helpers;

namespace rateit.Products;

public class NoteRequestModel : Request
{
    public NoteRequestModel(int productId, int userId, string note, string token) : base(token)
    {
        ProductId = productId;
        UserId = userId;
        Note = note;
    }

    public int ProductId { get; }
    public int UserId { get; }
    public string Note { get; }
}