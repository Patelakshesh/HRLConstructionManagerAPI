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
    public async Task<DashboardStats> GetDashboardStats([Service] AppDbContext context, string? dateFilter, string? siteName)
    {
        var sitesQuery = context.Sites.Where(s => s.Enable);
        int? targetSiteId = null;
        
        if (!string.IsNullOrEmpty(siteName) && !siteName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var siteObj = await context.Sites.FirstOrDefaultAsync(s => s.SiteName == siteName);
            if (siteObj != null)
            {
                targetSiteId = siteObj.Id;
                sitesQuery = sitesQuery.Where(s => s.Id == targetSiteId.Value);
            }
        }

        var activeSitesCount = await sitesQuery.CountAsync();
        var totalBudget = activeSitesCount * 500000m;

        var expenseQuery = context.Expenses.Where(e => e.Type == "Expense");
        if (targetSiteId.HasValue)
        {
            expenseQuery = expenseQuery.Where(e => e.SiteId == targetSiteId.Value);
        }

        if (!string.IsNullOrEmpty(dateFilter))
        {
            var now = DateTime.UtcNow;
            switch (dateFilter.ToLower())
            {
                case "today":
                    var todayStart = DateTime.Today.ToUniversalTime();
                    expenseQuery = expenseQuery.Where(e => e.Date >= todayStart);
                    break;
                case "last7days":
                    var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
                    expenseQuery = expenseQuery.Where(e => e.Date >= sevenDaysAgo);
                    break;
                case "last30days":
                    var thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);
                    expenseQuery = expenseQuery.Where(e => e.Date >= thirtyDaysAgo);
                    break;
                case "thismonth":
                    var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    expenseQuery = expenseQuery.Where(e => e.Date >= startOfMonth);
                    break;
            }
        }

        var totalExpenses = await expenseQuery.SumAsync(e => e.Amount);
        var remainingBudget = totalBudget - totalExpenses;

        // Dynamic trends for past 6 months
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var expenseTrends = new List<ExpenseTrend>();
        
        var trendQuery = context.Expenses.Where(e => e.Type == "Expense");
        if (targetSiteId.HasValue)
        {
            trendQuery = trendQuery.Where(e => e.SiteId == targetSiteId.Value);
        }

        var sixMonthsAgo = DateTime.UtcNow.Date.AddMonths(-5);
        var recentExpensesList = await trendQuery
            .Where(e => e.Date >= new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1, 0, 0, 0, DateTimeKind.Utc))
            .ToListAsync();

        for (int i = 5; i >= 0; i--)
        {
            var targetMonth = DateTime.UtcNow.AddMonths(-i);
            var monthName = months[targetMonth.Month - 1];
            var monthExpenses = recentExpensesList
                .Where(e => e.Date.Month == targetMonth.Month && e.Date.Year == targetMonth.Year)
                .Sum(e => e.Amount);
            
            var activeSiteCount = targetSiteId.HasValue ? 1 : activeSitesCount;
            var monthBudget = activeSiteCount * 50000m;
            
            expenseTrends.Add(new ExpenseTrend(monthName, monthExpenses, monthBudget));
        }

        // Budget comparisons list
        var sitesList = await sitesQuery.ToListAsync();
        var budgetComparisons = new List<SiteBudgetComparison>();
        foreach (var site in sitesList)
        {
            var siteExpenseQuery = context.Expenses.Where(e => e.SiteId == site.Id && e.Type == "Expense");
            if (!string.IsNullOrEmpty(dateFilter))
            {
                var now = DateTime.UtcNow;
                switch (dateFilter.ToLower())
                {
                    case "today":
                        var todayStart = DateTime.Today.ToUniversalTime();
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= todayStart);
                        break;
                    case "last7days":
                        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= sevenDaysAgo);
                        break;
                    case "last30days":
                        var thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= thirtyDaysAgo);
                        break;
                    case "thismonth":
                        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= startOfMonth);
                        break;
                }
            }
            var siteExpense = await siteExpenseQuery.SumAsync(e => e.Amount);
            budgetComparisons.Add(new SiteBudgetComparison(site.SiteName, 500000m, siteExpense));
        }

        return new DashboardStats(
            TotalBudget: totalBudget,
            TotalExpenses: totalExpenses,
            RemainingBudget: remainingBudget,
            ActiveSites: activeSitesCount,
            ExpenseTrends: expenseTrends,
            BudgetComparisons: budgetComparisons);
    }
}
