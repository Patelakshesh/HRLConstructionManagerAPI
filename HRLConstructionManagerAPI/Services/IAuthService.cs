using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface IAuthService
{
    AuthResponse Login(LoginInput input);
}
