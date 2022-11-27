using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public sealed class Subcategory : Entity
{
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
    public Category Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}