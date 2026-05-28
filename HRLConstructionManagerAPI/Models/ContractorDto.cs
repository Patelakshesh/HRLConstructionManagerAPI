namespace HRLConstructionManagerAPI.Models;

public sealed record ContractorDto(
    int Id,
    string ContractorName,
    string Email,
    string Phone,
    string? AssignedSites,
    bool Enable,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
