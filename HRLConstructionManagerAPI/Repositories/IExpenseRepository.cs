namespace HRLConstructionManagerAPI.Repositories;

using HRLConstructionManagerAPI.Entities;

public interface IExpenseRepository
{
    Task<Expense> CreateExpenseAsync(Expense expense);
    Task<Expense?> GetExpenseByIdAsync(int id);
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<(IEnumerable<Expense> Items, int TotalCount)> GetExpensesPageAsync(int pageNumber, int pageSize, string? search, int? siteId);
    Task<Expense> UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int id);
}
