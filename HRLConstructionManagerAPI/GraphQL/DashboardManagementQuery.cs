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

        var incomeQuery = context.Expenses.Where(e => e.Type == "Income");
        var expenseQuery = context.Expenses.Where(e => e.Type == "Expense");

        if (targetSiteId.HasValue)
        {
            incomeQuery = incomeQuery.Where(e => e.SiteId == targetSiteId.Value);
            expenseQuery = expenseQuery.Where(e => e.SiteId == targetSiteId.Value);
        }

        if (!string.IsNullOrEmpty(dateFilter))
        {
            var now = DateTime.UtcNow;
            switch (dateFilter.ToLower())
            {
                case "today":
                    var todayStart = DateTime.Today.ToUniversalTime();
                    incomeQuery = incomeQuery.Where(e => e.Date >= todayStart);
                    expenseQuery = expenseQuery.Where(e => e.Date >= todayStart);
                    break;
                case "last7days":
                    var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
                    incomeQuery = incomeQuery.Where(e => e.Date >= sevenDaysAgo);
                    expenseQuery = expenseQuery.Where(e => e.Date >= sevenDaysAgo);
                    break;
                case "last30days":
                    var thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);
                    incomeQuery = incomeQuery.Where(e => e.Date >= thirtyDaysAgo);
                    expenseQuery = expenseQuery.Where(e => e.Date >= thirtyDaysAgo);
                    break;
                case "thismonth":
                    var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    incomeQuery = incomeQuery.Where(e => e.Date >= startOfMonth);
                    expenseQuery = expenseQuery.Where(e => e.Date >= startOfMonth);
                    break;
            }
        }

        var totalBudget = await incomeQuery.SumAsync(e => e.Amount);
        var totalExpenses = await expenseQuery.SumAsync(e => e.Amount);
        var remainingBudget = totalBudget - totalExpenses;

        // Dynamic trends for past 6 months
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var expenseTrends = new List<ExpenseTrend>();
        
        var trendQuery = context.Expenses.Where(e => e.Type == "Expense");
        var budgetTrendQuery = context.Expenses.Where(e => e.Type == "Income");
        if (targetSiteId.HasValue)
        {
            trendQuery = trendQuery.Where(e => e.SiteId == targetSiteId.Value);
            budgetTrendQuery = budgetTrendQuery.Where(e => e.SiteId == targetSiteId.Value);
        }

        var sixMonthsAgo = DateTime.UtcNow.Date.AddMonths(-5);
        var minDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        
        var recentExpensesList = await trendQuery
            .Where(e => e.Date >= minDate)
            .ToListAsync();
            
        var recentBudgetsList = await budgetTrendQuery
            .Where(e => e.Date >= minDate)
            .ToListAsync();

        for (int i = 5; i >= 0; i--)
        {
            var targetMonth = DateTime.UtcNow.AddMonths(-i);
            var monthName = months[targetMonth.Month - 1];
            var monthExpenses = recentExpensesList
                .Where(e => e.Date.Month == targetMonth.Month && e.Date.Year == targetMonth.Year)
                .Sum(e => e.Amount);
            
            var monthBudget = recentBudgetsList
                .Where(e => e.Date.Month == targetMonth.Month && e.Date.Year == targetMonth.Year)
                .Sum(e => e.Amount);
            
            expenseTrends.Add(new ExpenseTrend(monthName, monthExpenses, monthBudget));
        }

        // Budget comparisons list
        var sitesList = await sitesQuery.ToListAsync();
        var budgetComparisons = new List<SiteBudgetComparison>();
        foreach (var site in sitesList)
        {
            var siteExpenseQuery = context.Expenses.Where(e => e.SiteId == site.Id && e.Type == "Expense");
            var siteIncomeQuery = context.Expenses.Where(e => e.SiteId == site.Id && e.Type == "Income");
            if (!string.IsNullOrEmpty(dateFilter))
            {
                var now = DateTime.UtcNow;
                switch (dateFilter.ToLower())
                {
                    case "today":
                        var todayStart = DateTime.Today.ToUniversalTime();
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= todayStart);
                        siteIncomeQuery = siteIncomeQuery.Where(e => e.Date >= todayStart);
                        break;
                    case "last7days":
                        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= sevenDaysAgo);
                        siteIncomeQuery = siteIncomeQuery.Where(e => e.Date >= sevenDaysAgo);
                        break;
                    case "last30days":
                        var thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= thirtyDaysAgo);
                        siteIncomeQuery = siteIncomeQuery.Where(e => e.Date >= thirtyDaysAgo);
                        break;
                    case "thismonth":
                        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                        siteExpenseQuery = siteExpenseQuery.Where(e => e.Date >= startOfMonth);
                        siteIncomeQuery = siteIncomeQuery.Where(e => e.Date >= startOfMonth);
                        break;
                }
            }
            var siteExpense = await siteExpenseQuery.SumAsync(e => e.Amount);
            var siteIncome = await siteIncomeQuery.SumAsync(e => e.Amount);
            budgetComparisons.Add(new SiteBudgetComparison(site.SiteName, siteIncome, siteExpense));
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
