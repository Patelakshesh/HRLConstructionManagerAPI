namespace HRLConstructionManagerAPI.Models;

public sealed record CreateCategoryInput(
    string Name,
    string? Description,
    bool Enable,
    string? CreatedBy);

public sealed record UpdateCategoryInput(
    int Id,
    string Name,
    string? Description,
    bool Enable,
    string? ModifiedBy);
