namespace HRLConstructionManagerAPI.GraphQL;

using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;
using HotChocolate.Authorization;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class AttendanceManagementMutation
{
    [Authorize]
    public async Task<Attendance> CreateAttendance(
        CreateAttendanceInput input,
        [Service] IAttendanceRepository attendanceRepository)
    {
        var attendance = new Attendance
        {
            Date = input.Date,
            SiteId = input.SiteId,
            ContractorId = input.ContractorId,
            SupervisorId = input.SupervisorId,
            SkilledWorkers = input.SkilledWorkers,
            SemiSkilledWorkers = input.SemiSkilledWorkers,
            UnskilledWorkers = input.UnskilledWorkers,
            StartTime = input.StartTime,
            EndTime = input.EndTime,
            Image = input.Image,
            CreatedBy = input.CreatedBy
        };

        return await attendanceRepository.CreateAttendanceAsync(attendance);
    }

    [Authorize]
    public async Task<Attendance?> UpdateAttendance(
        UpdateAttendanceInput input,
        [Service] IAttendanceRepository attendanceRepository)
    {
        var attendance = await attendanceRepository.GetAttendanceByIdAsync(input.Id);
        if (attendance == null)
        {
            return null;
        }

        attendance.Date = input.Date;
        attendance.SiteId = input.SiteId;
        attendance.ContractorId = input.ContractorId;
        attendance.SupervisorId = input.SupervisorId;
        attendance.SkilledWorkers = input.SkilledWorkers;
        attendance.SemiSkilledWorkers = input.SemiSkilledWorkers;
        attendance.UnskilledWorkers = input.UnskilledWorkers;
        attendance.StartTime = input.StartTime;
        attendance.EndTime = input.EndTime;
        attendance.Image = input.Image;
        attendance.ModifiedBy = input.ModifiedBy;
        attendance.ModifiedOn = DateTime.UtcNow;

        return await attendanceRepository.UpdateAttendanceAsync(attendance);
    }

    [Authorize]
    public async Task<bool> DeleteAttendance(
        int id,
        [Service] IAttendanceRepository attendanceRepository)
    {
        return await attendanceRepository.DeleteAttendanceAsync(id);
    }
}
