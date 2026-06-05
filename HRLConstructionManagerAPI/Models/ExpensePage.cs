namespace HRLConstructionManagerAPI.Models;

using HRLConstructionManagerAPI.Entities;

public sealed record ExpensePage(
    IReadOnlyCollection<Expense> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
