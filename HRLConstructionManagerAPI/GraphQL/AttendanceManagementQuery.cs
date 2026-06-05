namespace HRLConstructionManagerAPI.GraphQL;

using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;
using HotChocolate.Authorization;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class AttendanceManagementQuery
{
    [Authorize]
    public async Task<IReadOnlyCollection<Attendance>> GetAttendances([Service] IAttendanceRepository attendanceRepository) =>
        (await attendanceRepository.GetAllAttendancesAsync()).ToList().AsReadOnly();

    [Authorize]
    public async Task<AttendancePage> GetAttendancesPage(
        int pageNumber,
        int pageSize,
        string? search,
        int? siteId,
        DateTime? startDate,
        DateTime? endDate,
        [Service] IAttendanceRepository attendanceRepository)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = await attendanceRepository.GetAttendancesPageAsync(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch,
            siteId,
            startDate,
            endDate);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new AttendancePage(
            items.ToList().AsReadOnly(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    [Authorize]
    public async Task<Attendance?> GetAttendance(int id, [Service] IAttendanceRepository attendanceRepository) =>
        await attendanceRepository.GetAttendanceByIdAsync(id);
}
