namespace HRLConstructionManagerAPI.Entities;

public class SupervisorCredit
{
    public int Id { get; set; }

    public required string SupervisorName { get; set; }

    public decimal Amount { get; set; }

    public required string PaymentMode { get; set; } // "Cash", "Check", "Online"

    public string? TransactionId { get; set; }

    public string? Comment { get; set; }

    public DateTime Date { get; set; }

    public string? ReceiptImage { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
