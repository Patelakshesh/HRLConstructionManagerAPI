namespace HRLConstructionManagerAPI.Models;

public sealed record ContractorPage(
    IReadOnlyCollection<ContractorDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages);
