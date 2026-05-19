namespace HRLConstructionManagerAPI.Models;

public sealed record UpdateUserInput(
    int Id,
    string MobileNumber,
    string Name,
    int RoleId,
    string Address,
    string? Email,
    string? ModifiedBy);
