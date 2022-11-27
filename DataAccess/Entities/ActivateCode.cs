using rateit.DataAccess.Abstract;

namespace rateit.DataAccess.Entities;

public sealed class ActivateCode : Entity
{
    public string Code { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}