using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class CategoryDTO
{
    public string? Name { get; set; }
    public ICollection<Subcategory> Subcategories { get; set; }
    public Guid Id { get; set; }
}