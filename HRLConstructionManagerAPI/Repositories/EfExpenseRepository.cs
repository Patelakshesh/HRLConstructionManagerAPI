namespace HRLConstructionManagerAPI.Repositories;

using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

public class EfExpenseRepository(AppDbContext context) : IExpenseRepository
{
    public async Task<Expense> CreateExpenseAsync(Expense expense)
    {
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int id)
    {
        return await context.Expenses
            .Include(e => e.Site)
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        var temp = await context.Expenses
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.SiteId,
                e.Site,
                e.CategoryId,
                e.Category,
                e.Amount,
                e.PaymentMode,
                e.TransactionId,
                e.Date,
                e.Type,
                e.CreatedOn,
                e.CreatedBy,
                e.ModifiedOn,
                e.ModifiedBy
            })
            .OrderByDescending(e => e.Date)
            .ToListAsync();

        return temp.Select(e => new Expense
        {
            Id = e.Id,
            Title = e.Title,
            SiteId = e.SiteId,
            Site = e.Site,
            CategoryId = e.CategoryId,
            Category = e.Category,
            Amount = e.Amount,
            PaymentMode = e.PaymentMode,
            TransactionId = e.TransactionId,
            Date = e.Date,
            Type = e.Type,
            ReceiptImage = null,
            CreatedOn = e.CreatedOn,
            CreatedBy = e.CreatedBy,
            ModifiedOn = e.ModifiedOn,
            ModifiedBy = e.ModifiedBy
        });
    }

    public async Task<(IEnumerable<Expense> Items, int TotalCount)> GetExpensesPageAsync(int pageNumber, int pageSize, string? search, int? siteId)
    {
        var query = context.Expenses
            .Include(e => e.Site)
            .Include(e => e.Category)
            .AsQueryable();

        if (siteId.HasValue)
        {
            query = query.Where(e => e.SiteId == siteId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(e =>
                e.Title.ToLower().Contains(searchLower) ||
                (e.TransactionId != null && e.TransactionId.ToLower().Contains(searchLower)) ||
                (e.Site != null && e.Site.SiteName.ToLower().Contains(searchLower)) ||
                (e.Category != null && e.Category.Name.ToLower().Contains(searchLower)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Expense> UpdateExpenseAsync(Expense expense)
    {
        context.Expenses.Update(expense);
        await context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await context.Expenses.FindAsync(id);
        if (expense == null)
        {
            return false;
        }

        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
        return true;
    }
}
