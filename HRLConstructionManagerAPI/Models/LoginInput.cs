namespace HRLConstructionManagerAPI.Models;

public sealed record LoginInput(
    string MobileNumber,
    string Password);
