namespace HRLConstructionManagerAPI.Entities;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool Enable { get; set; } = true;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
