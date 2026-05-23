namespace HRLConstructionManagerAPI.Models;

public sealed record CreateSupervisorCreditInput(
    string SupervisorName,
    decimal Amount,
    string PaymentMode,
    string? TransactionId,
    string? Comment,
    DateTime Date,
    string? CreatedBy);

public sealed record UpdateSupervisorCreditInput(
    int Id,
    string SupervisorName,
    decimal Amount,
    string PaymentMode,
    string? TransactionId,
    string? Comment,
    DateTime Date,
    string? ModifiedBy);
