namespace HRLConstructionManagerAPI.Models;

public record CreateAttendanceInput(
    DateTime Date,
    int SiteId,
    int ContractorId,
    int SupervisorId,
    int SkilledWorkers,
    int SemiSkilledWorkers,
    int UnskilledWorkers,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? CreatedBy);

public record UpdateAttendanceInput(
    int Id,
    DateTime Date,
    int SiteId,
    int ContractorId,
    int SupervisorId,
    int SkilledWorkers,
    int SemiSkilledWorkers,
    int UnskilledWorkers,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? ModifiedBy);
