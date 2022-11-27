using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class NotedProductDTO
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string? Note { get; set; }
}