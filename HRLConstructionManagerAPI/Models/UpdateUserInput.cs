using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record UpdateUserInput(
    [Required] int Id,
    [Required] [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")] string MobileNumber,
    [Required] [MaxLength(100)] string Name,
    [Required] int RoleId,
    [Required] [MaxLength(250)] string Address,
    [EmailAddress] [MaxLength(150)] string? Email,
    [MinLength(6)] string? Password,
    bool Enable,
    string? ModifiedBy);
