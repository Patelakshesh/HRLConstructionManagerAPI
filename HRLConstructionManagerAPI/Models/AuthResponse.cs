namespace HRLConstructionManagerAPI.Models;

public sealed record AuthResponse(
    string Token,
    DateTime ExpiresOn,
    UserDto User);
