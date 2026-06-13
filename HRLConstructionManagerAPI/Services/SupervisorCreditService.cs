using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class SupervisorCreditService(ISupervisorCreditRepository creditRepository) : ISupervisorCreditService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public IReadOnlyCollection<SupervisorCredit> GetSupervisorCredits() => creditRepository.GetAll();

    public SupervisorCreditPage GetSupervisorCreditsPage(
        int pageNumber, 
        int pageSize, 
        string? search, 
        string? supervisorName, 
        string? paymentMode)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);
            
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
        var normalizedSupervisor = string.IsNullOrWhiteSpace(supervisorName) ? null : supervisorName.Trim();
        var normalizedPaymentMode = string.IsNullOrWhiteSpace(paymentMode) ? null : paymentMode.Trim();

        var (items, totalCount) = creditRepository.GetPaged(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch,
            normalizedSupervisor,
            normalizedPaymentMode);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new SupervisorCreditPage(
            items.Select(ToDto).ToArray(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    public SupervisorCredit? GetSupervisorCredit(int id) => creditRepository.GetById(id);

    public SupervisorCredit CreateSupervisorCredit(CreateSupervisorCreditInput input)
    {
        var paymentMode = input.PaymentMode.Trim();
        ValidatePaymentMode(paymentMode, input.TransactionId);

        var credit = BuildCredit(input.SupervisorName, input.Amount, paymentMode, input.TransactionId, input.Comment, input.Date, input.ReceiptImage);
        credit.CreatedBy = input.CreatedBy;

        return creditRepository.Add(credit);
    }

    public SupervisorCredit? UpdateSupervisorCredit(UpdateSupervisorCreditInput input)
    {
        var paymentMode = input.PaymentMode.Trim();
        ValidatePaymentMode(paymentMode, input.TransactionId);

        var credit = BuildCredit(input.SupervisorName, input.Amount, paymentMode, input.TransactionId, input.Comment, input.Date, input.ReceiptImage);
        credit.Id = input.Id;
        credit.ModifiedBy = input.ModifiedBy;

        return creditRepository.Update(credit);
    }

    private static SupervisorCredit BuildCredit(
        string supervisorName, decimal amount, string paymentMode,
        string? transactionId, string? comment, DateTime date, string? receiptImage) =>
        new()
        {
            SupervisorName = supervisorName.Trim(),
            Amount = amount,
            PaymentMode = paymentMode,
            TransactionId = string.IsNullOrWhiteSpace(transactionId) ? null : transactionId.Trim(),
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
            Date = date,
            ReceiptImage = receiptImage
        };

    public bool DeleteSupervisorCredit(int id) => creditRepository.Delete(id);

    private static void ValidatePaymentMode(string paymentMode, string? transactionId)
    {
        if (paymentMode != "Cash" && paymentMode != "Check" && paymentMode != "Online")
        {
            throw new ArgumentException("Payment mode must be Cash, Check, or Online.");
        }

        if (paymentMode != "Cash" && string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException($"Transaction ID is required when payment mode is {paymentMode}.");
        }
    }


    private static SupervisorCreditDto ToDto(SupervisorCredit credit) =>
        new(
            credit.Id,
            credit.SupervisorName,
            credit.Amount,
            credit.PaymentMode,
            credit.TransactionId,
            credit.Comment,
            credit.Date,
            credit.ReceiptImage,
            credit.CreatedOn,
            credit.CreatedBy,
            credit.ModifiedOn,
            credit.ModifiedBy);
}
