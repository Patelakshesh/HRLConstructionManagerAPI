using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record CreateSupervisorCreditInput(
    [Required] [MaxLength(150)] string SupervisorName,
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")] decimal Amount,
    [Required] [MaxLength(50)] string PaymentMode,
    [MaxLength(100)] string? TransactionId,
    [MaxLength(500)] string? Comment,
    [Required] DateTime Date,
    string? ReceiptImage,
    string? CreatedBy);

public sealed record UpdateSupervisorCreditInput(
    [Required] int Id,
    [Required] [MaxLength(150)] string SupervisorName,
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")] decimal Amount,
    [Required] [MaxLength(50)] string PaymentMode,
    [MaxLength(100)] string? TransactionId,
    [MaxLength(500)] string? Comment,
    [Required] DateTime Date,
    string? ReceiptImage,
    string? ModifiedBy);
