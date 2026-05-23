namespace HRLConstructionManagerAPI.Models;

public sealed record SupervisorCreditPage(
    IReadOnlyCollection<SupervisorCreditDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
