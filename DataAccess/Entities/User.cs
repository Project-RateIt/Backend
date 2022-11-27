using rateit.DataAccess.Abstract;
namespace rateit.DataAccess.Entities;

public sealed class User : Entity
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? ResetPassKey { get; set; }
    public int? AddedProduct { get; set; }
    public bool IsActive { get; set; }
    public bool HaveAvatar { get; set; }
    public ActivateCode? ActivateCode { get; set; }
    public AccountType AccountType { get; set; }
    public ICollection<NotedProduct>? NotedProducts { get; set; }
    public ICollection<RatedProduct>? RatedProducts { get; set; }
    public ICollection<ViewedProduct>? ViewedProducts { get; set; }
}