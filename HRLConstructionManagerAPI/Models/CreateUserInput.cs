namespace HRLConstructionManagerAPI.Models;

public sealed record CreateUserInput(
    string MobileNumber,
    string Name,
    string Password,
    int RoleId,
    string Address,
    string? Email,
    string? CreatedBy);
