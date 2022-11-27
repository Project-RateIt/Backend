using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class ViewedProductDTO
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public string Date { get; set; }
}