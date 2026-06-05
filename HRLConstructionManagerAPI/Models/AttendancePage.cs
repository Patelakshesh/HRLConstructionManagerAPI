namespace HRLConstructionManagerAPI.Models;

using HRLConstructionManagerAPI.Entities;

public sealed record AttendancePage(
    IReadOnlyCollection<Attendance> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
