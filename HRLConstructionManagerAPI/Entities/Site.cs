namespace HRLConstructionManagerAPI.Entities;

public class Site
{
    public int Id { get; set; }

    public required string SiteName { get; set; }

    public required string Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Enable { get; set; } = true;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
