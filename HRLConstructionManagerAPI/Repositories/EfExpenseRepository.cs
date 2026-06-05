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
        return await context.Expenses
            .Include(e => e.Site)
            .Include(e => e.Category)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
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
