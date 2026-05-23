namespace HRLConstructionManagerAPI.Models;

public sealed record SitePage(
    IReadOnlyCollection<SiteDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
