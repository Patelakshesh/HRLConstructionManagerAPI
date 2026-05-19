namespace HRLConstructionManagerAPI.Models;

public sealed record UpdateRoleInput(
    int Id,
    string RoleName,
    bool Enable);
