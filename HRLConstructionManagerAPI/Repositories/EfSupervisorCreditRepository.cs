using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfSupervisorCreditRepository(AppDbContext dbContext) : ISupervisorCreditRepository
{
    public IReadOnlyCollection<SupervisorCredit> GetAll() =>
        dbContext.SupervisorCredits
            .AsNoTracking()
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.Id)
            .ToArray();

    public (IReadOnlyCollection<SupervisorCredit> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search,
        string? supervisorName,
        string? paymentMode)
    {
        var query = dbContext.SupervisorCredits.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                (c.Comment != null && c.Comment.Contains(search)) ||
                c.SupervisorName.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(supervisorName) && supervisorName != "all")
        {
            query = query.Where(c => c.SupervisorName == supervisorName);
        }

        if (!string.IsNullOrWhiteSpace(paymentMode) && paymentMode != "all")
        {
            query = query.Where(c => c.PaymentMode == paymentMode);
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return (items, totalCount);
    }

    public SupervisorCredit? GetById(int id) =>
        dbContext.SupervisorCredits
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);

    public SupervisorCredit Add(SupervisorCredit credit)
    {
        dbContext.SupervisorCredits.Add(credit);
        dbContext.SaveChanges();
        return credit;
    }

    public SupervisorCredit? Update(SupervisorCredit credit)
    {
        var existing = dbContext.SupervisorCredits.Find(credit.Id);
        if (existing is null)
        {
            return null;
        }

        existing.SupervisorName = credit.SupervisorName;
        existing.Amount = credit.Amount;
        existing.PaymentMode = credit.PaymentMode;
        existing.TransactionId = credit.TransactionId;
        existing.Comment = credit.Comment;
        existing.Date = credit.Date;
        existing.ReceiptImage = credit.ReceiptImage;
        existing.ModifiedOn = DateTime.UtcNow;
        existing.ModifiedBy = credit.ModifiedBy;

        dbContext.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var existing = dbContext.SupervisorCredits.Find(id);
        if (existing is null)
        {
            return false;
        }

        dbContext.SupervisorCredits.Remove(existing);
        dbContext.SaveChanges();
        return true;
    }
}
