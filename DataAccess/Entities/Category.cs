using Microsoft.AspNetCore.Identity;
using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public sealed class Category : Entity
{
    public string? Name { get; set; }
    public ICollection<Subcategory> Subcategories { get; set; }
    public ICollection<Product> Products { get; set; }
}