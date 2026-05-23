namespace HRLConstructionManagerAPI.Models;

public sealed record CreateContractorInput(
    string CompanyName,
    string ContactPerson,
    string Email,
    string Phone,
    string? AssignedSites,
    bool Enable,
    string? CreatedBy);

public sealed record UpdateContractorInput(
    int Id,
    string CompanyName,
    string ContactPerson,
    string Email,
    string Phone,
    string? AssignedSites,
    bool Enable,
    string? ModifiedBy);
