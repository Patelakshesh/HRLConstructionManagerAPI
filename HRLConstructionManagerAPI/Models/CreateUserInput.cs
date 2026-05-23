using System.ComponentModel.DataAnnotations;

namespace HRLConstructionManagerAPI.Models;

public sealed record CreateUserInput(
    [Required] [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")] string MobileNumber,
    [Required] [MaxLength(100)] string Name,
    [Required] [MinLength(6)] string Password,
    [Required] int RoleId,
    [Required] [MaxLength(250)] string Address,
    [EmailAddress] [MaxLength(150)] string? Email,
    bool Enable = true,
    string? CreatedBy = null);
