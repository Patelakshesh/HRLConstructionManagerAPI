using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record CreateCategoryInput(
    [Required] [MaxLength(100)] string Name,
    [Required] [MaxLength(500)] string? Description,
    bool Enable,
    string? CreatedBy);

public sealed record UpdateCategoryInput(
    [Required] int Id,
    [Required] [MaxLength(100)] string Name,
    [Required] [MaxLength(500)] string? Description,
    bool Enable,
    string? ModifiedBy);
