namespace HRLConstructionManagerAPI.Models;

public sealed record SiteDto(
    int Id,
    string SiteName,
    string Address,
    string? City,
    string? State,
    string? ContactPerson,
    string? ContactNumber,
    DateTime? StartDate,
    DateTime? EndDate,
    bool Enable,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
