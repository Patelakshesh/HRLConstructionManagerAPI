using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

//[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class AuthMutation
{
    [AllowAnonymous]
    public AuthResponse Login(LoginInput input, [Service] IAuthService authService) =>
        authService.Login(input);
}
