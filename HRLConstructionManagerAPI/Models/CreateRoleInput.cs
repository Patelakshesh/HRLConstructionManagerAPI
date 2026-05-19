namespace HRLConstructionManagerAPI.Models;

public sealed record CreateRoleInput(
    string RoleName,
    bool Enable = true);
