namespace HRLConstructionManagerAPI.Models;

public sealed record UserDto(
    int Id,
    string MobileNumber,
    string Name,
    int RoleId,
    string Address,
    string? Email,
    bool Enable,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
