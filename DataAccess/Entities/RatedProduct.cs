using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public class RatedProduct : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Rate { get; set; }
}