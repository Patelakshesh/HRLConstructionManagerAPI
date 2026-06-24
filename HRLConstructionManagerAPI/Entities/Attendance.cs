namespace HRLConstructionManagerAPI.Entities;

public class Attendance
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int SiteId { get; set; }
    public Site? Site { get; set; }

    public int ContractorId { get; set; }
    public Contractor? Contractor { get; set; }

    public int SupervisorId { get; set; }
    public User? Supervisor { get; set; }

    public int SkilledWorkers { get; set; }

    public int SemiSkilledWorkers { get; set; }

    public int UnskilledWorkers { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
    
    public string? Image { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
