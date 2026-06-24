namespace HRLConstructionManagerAPI.GraphQL;

using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;
using HotChocolate.Authorization;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class ExpenseManagementQuery
{
    [Authorize]
    public async Task<IReadOnlyCollection<Expense>> GetExpenses(
        DateTime? startDate,
        DateTime? endDate,
        [Service] IExpenseRepository expenseRepository) =>
        (await expenseRepository.GetAllExpensesAsync(startDate, endDate)).ToList().AsReadOnly();

    [Authorize]
    public async Task<ExpensePage> GetExpensesPage(
        int pageNumber,
        int pageSize,
        string? search,
        int? siteId,
        [Service] IExpenseRepository expenseRepository)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = await expenseRepository.GetExpensesPageAsync(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch,
            siteId);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new ExpensePage(
            items.ToList().AsReadOnly(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    [Authorize]
    public async Task<Expense?> GetExpense(int id, [Service] IExpenseRepository expenseRepository) =>
        await expenseRepository.GetExpenseByIdAsync(id);
}
