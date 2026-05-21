namespace HRLConstructionManagerAPI.Models;

public sealed record CreateUserInput(
    string MobileNumber,
    string Name,
    string Password,
    int RoleId,
    string Address,
    string? Email,
    bool Enable = true,
    string? CreatedBy = null);
