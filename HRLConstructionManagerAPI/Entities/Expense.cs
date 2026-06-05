namespace HRLConstructionManagerAPI.Entities;

public class Expense
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int? SiteId { get; set; }
    public Site? Site { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public decimal Amount { get; set; }

    public required string PaymentMode { get; set; }

    public string? TransactionId { get; set; }

    public DateTime Date { get; set; }

    public required string Type { get; set; } // Income or Expense

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
