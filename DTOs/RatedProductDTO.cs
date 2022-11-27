using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class RatedProductDTO
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Rate { get; set; }
}