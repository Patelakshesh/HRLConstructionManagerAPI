namespace HRLConstructionManagerAPI.Repositories;

using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

public class EfAttendanceRepository(AppDbContext context) : IAttendanceRepository
{
    public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
    {
        context.Attendances.Add(attendance);
        await context.SaveChangesAsync();
        return attendance;
    }

    public async Task<Attendance?> GetAttendanceByIdAsync(int id)
    {
        return await context.Attendances
            .Include(a => a.Site)
            .Include(a => a.Contractor)
            .Include(a => a.Supervisor)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = context.Attendances
            .Include(a => a.Site)
            .Include(a => a.Contractor)
            .Include(a => a.Supervisor)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Date <= endDate.Value);
        }

        return await query.OrderByDescending(a => a.Date).ToListAsync();
    }

    public async Task<(IEnumerable<Attendance> Items, int TotalCount)> GetAttendancesPageAsync(
        int pageNumber,
        int pageSize,
        string? search,
        int? siteId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = context.Attendances
            .Include(a => a.Site)
            .Include(a => a.Contractor)
            .Include(a => a.Supervisor)
            .AsQueryable();

        if (siteId.HasValue)
        {
            query = query.Where(a => a.SiteId == siteId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Date <= endDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(a =>
                (a.Site != null && a.Site.SiteName.ToLower().Contains(searchLower)) ||
                (a.Contractor != null && a.Contractor.ContractorName.ToLower().Contains(searchLower)) ||
                (a.Supervisor != null && a.Supervisor.Name.ToLower().Contains(searchLower)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
    {
        context.Attendances.Update(attendance);
        await context.SaveChangesAsync();
        return attendance;
    }

    public async Task<bool> DeleteAttendanceAsync(int id)
    {
        var attendance = await context.Attendances.FindAsync(id);
        if (attendance == null)
        {
            return false;
        }

        context.Attendances.Remove(attendance);
        await context.SaveChangesAsync();
        return true;
    }
}
