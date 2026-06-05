namespace HRLConstructionManagerAPI.Repositories;

using HRLConstructionManagerAPI.Entities;

public interface IAttendanceRepository
{
    Task<Attendance> CreateAttendanceAsync(Attendance attendance);
    Task<Attendance?> GetAttendanceByIdAsync(int id);
    Task<IEnumerable<Attendance>> GetAllAttendancesAsync();
    Task<(IEnumerable<Attendance> Items, int TotalCount)> GetAttendancesPageAsync(int pageNumber, int pageSize, string? search, int? siteId, DateTime? startDate, DateTime? endDate);
    Task<Attendance> UpdateAttendanceAsync(Attendance attendance);
    Task<bool> DeleteAttendanceAsync(int id);
}
