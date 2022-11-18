using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public sealed class ViewedProduct : Entity
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;
    public string Date { get; set; }
}