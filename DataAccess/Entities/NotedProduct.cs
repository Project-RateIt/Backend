using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public sealed class NotedProduct : Entity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } 
    public string? Note { get; set; }
}