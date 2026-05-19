namespace HRLConstructionManagerAPI.Models;

public sealed record UserDto(
    int Id,
    string MobileNumber,
    string Name,
    int RoleId,
    string Address,
    string? Email,
    DateTime CreatedOn,
    string? CreatedBy,
    DateTime? ModifiedOn,
    string? ModifiedBy);
