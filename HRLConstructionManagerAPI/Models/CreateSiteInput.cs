using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record CreateSiteInput(
    [Required] [MaxLength(150)] string SiteName,
    [Required] [MaxLength(250)] string Address,
    [MaxLength(100)] string? City,
    [MaxLength(100)] string? State,
    [MaxLength(100)] string? ContactPerson,
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")] string? ContactNumber,
    DateTime? StartDate,
    DateTime? EndDate,
    bool Enable,
    string? CreatedBy);
