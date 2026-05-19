namespace HRLConstructionManagerAPI.Entities;

public class Role
{
    public int Id { get; set; }

    public required string RoleName { get; set; }

    public bool Enable { get; set; } = true;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }

    public ICollection<User> Users { get; set; } = [];
}
