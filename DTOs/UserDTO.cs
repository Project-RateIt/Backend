using rateit.DataAccess.Entities;

namespace rateit.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public int AddedProduct { get; set; }
    public bool HaveAvatar { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
    public AccountType AccountType { get; set; }
    public ActivateCode ActivateCode { get; set; }

    public ICollection<NotedProductDTO> NotedProducts { get; set; }
    public ICollection<RatedProductDTO> RatedProducts { get; set; }
    public ICollection<ViewedProductDTO> ViewedProducts { get; set; }
}