namespace HRLConstructionManagerAPI.Entities;

public class User
{
    public int Id { get; set; }

    public required string MobileNumber { get; set; }

    public required string Name { get; set; }

    public string Password { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public Role? Role { get; set; }

    public bool Enable { get; set; } = true;

    public required string Address { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
