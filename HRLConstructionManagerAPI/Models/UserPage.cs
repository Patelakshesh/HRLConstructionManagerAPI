namespace HRLConstructionManagerAPI.Models;

public sealed record UserPage(
    IReadOnlyCollection<UserDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
