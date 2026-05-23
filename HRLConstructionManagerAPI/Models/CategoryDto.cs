namespace HRLConstructionManagerAPI.Models;

public sealed record CategoryDto(
    int Id,
    string Name,
    string? Description,
    bool Enable,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
