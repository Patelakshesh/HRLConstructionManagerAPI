namespace HRLConstructionManagerAPI.Models;

public record CreateExpenseInput(
    string Title,
    int? SiteId,
    int? CategoryId,
    decimal Amount,
    string PaymentMode,
    string? TransactionId,
    DateTime Date,
    string Type,
    string? CreatedBy);

public record UpdateExpenseInput(
    int Id,
    string Title,
    int? SiteId,
    int? CategoryId,
    decimal Amount,
    string PaymentMode,
    string? TransactionId,
    DateTime Date,
    string Type,
    string? ModifiedBy);
