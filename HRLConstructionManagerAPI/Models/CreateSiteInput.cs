namespace HRLConstructionManagerAPI.Models;

public sealed record CreateSiteInput(
    string SiteName,
    string Address,
    string? City,
    string? State,
    string? ContactPerson,
    string? ContactNumber,
    DateTime? StartDate,
    DateTime? EndDate,
    bool Enable,
    string? CreatedBy);
