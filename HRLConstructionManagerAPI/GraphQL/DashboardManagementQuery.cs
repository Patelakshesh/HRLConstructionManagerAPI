namespace HRLConstructionManagerAPI.GraphQL;

using HRLConstructionManagerAPI.Data;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

public sealed record DashboardStats(
    decimal TotalBudget,
    decimal TotalExpenses,
    decimal RemainingBudget,
    int ActiveSites,
    IReadOnlyCollection<ExpenseTrend> ExpenseTrends,
    IReadOnlyCollection<SiteBudgetComparison> BudgetComparisons);

public sealed record ExpenseTrend(string Month, decimal Expenses, decimal Budget);

public sealed record SiteBudgetComparison(string Site, decimal Budget, decimal Actual);

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class DashboardManagementQuery
{
    [Authorize]
    public async Task<DashboardStats> GetDashboardStats([Service] AppDbContext context)
    {
        var activeSites = await context.Sites.Where(s => s.Enable).CountAsync();
        
        // In a real app, Budget might be a property of Site or a separate entity. 
        // For now, we will mock a fixed budget per active site (e.g. 500,000) or calculate it.
        var totalBudget = activeSites * 500000m; 
        
        var totalExpenses = await context.Expenses
            .Where(e => e.Type == "Expense")
            .SumAsync(e => e.Amount);
            
        var remainingBudget = totalBudget - totalExpenses;

        // Mock trends and comparisons since budget isn't fully tracked per month in the db yet
        // In a complete system, we'd group expenses by month.
        var expenseTrends = new List<ExpenseTrend>
        {
            new("Jan", 45000, 50000),
            new("Feb", 52000, 55000),
            new("Mar", 48000, 50000),
            new("Apr", 61000, 60000),
            new("May", 55000, 65000),
            new("Jun", 67000, 70000)
        };

        var sites = await context.Sites.Where(s => s.Enable).ToListAsync();
        var budgetComparisons = new List<SiteBudgetComparison>();
        foreach (var site in sites)
        {
            var siteExpense = await context.Expenses
                .Where(e => e.SiteId == site.Id && e.Type == "Expense")
                .SumAsync(e => e.Amount);
            budgetComparisons.Add(new SiteBudgetComparison(site.SiteName, 500000m, siteExpense));
        }

        return new DashboardStats(
            TotalBudget: totalBudget,
            TotalExpenses: totalExpenses,
            RemainingBudget: remainingBudget,
            ActiveSites: activeSites,
            ExpenseTrends: expenseTrends,
            BudgetComparisons: budgetComparisons);
    }
}
