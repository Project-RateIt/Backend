using System.ComponentModel.DataAnnotations.Schema;
using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public class Product : Entity
{
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Producer { get; set; }
    public string? Ean { get; set; }

    public Guid SubcategoryId { get; set; }
    public Subcategory? Subcategory { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int RateSum { get; set; }
    public int RateCount { get; set; }
    public int Sponsor { get; set; }
    
    public ICollection<NotedProduct>? NotedProducts { get; set; }
    public ICollection<RatedProduct>? RatedProducts { get; set; }
    public ICollection<ViewedProduct>? ViewedProducts { get; set; }
}