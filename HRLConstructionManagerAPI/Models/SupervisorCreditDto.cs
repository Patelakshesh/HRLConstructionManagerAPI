namespace HRLConstructionManagerAPI.Models;

public sealed record SupervisorCreditDto(
    int Id,
    string SupervisorName,
    decimal Amount,
    string PaymentMode,
    string? TransactionId,
    string? Comment,
    DateTime Date,
    string? ReceiptImage,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
