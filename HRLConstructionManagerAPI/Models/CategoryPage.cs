namespace HRLConstructionManagerAPI.Models;

public sealed record CategoryPage(
    IReadOnlyCollection<CategoryDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
