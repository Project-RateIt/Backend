using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class SubcategoryDTO
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
    public Category Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}