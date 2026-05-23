namespace HRLConstructionManagerAPI.Entities;

public class Contractor
{
    public int Id { get; set; }

    public required string CompanyName { get; set; }

    public required string ContactPerson { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }

    /// <summary>Comma-separated list of assigned site names.</summary>
    public string? AssignedSites { get; set; }

    public bool Enable { get; set; } = true;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
