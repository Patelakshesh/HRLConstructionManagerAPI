using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record CreateContractorInput(
    [Required] [MaxLength(100)] string CompanyName,
    [Required] [MaxLength(100)] string ContactPerson,
    [Required] [EmailAddress] [MaxLength(150)] string Email,
    [Required] [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")] string Phone,
    [MaxLength(250)] string? AssignedSites,
    bool Enable,
    string? CreatedBy);

public sealed record UpdateContractorInput(
    [Required] int Id,
    [Required] [MaxLength(100)] string CompanyName,
    [Required] [MaxLength(100)] string ContactPerson,
    [Required] [EmailAddress] [MaxLength(150)] string Email,
    [Required] [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")] string Phone,
    [MaxLength(250)] string? AssignedSites,
    bool Enable,
    string? ModifiedBy);
