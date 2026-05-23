using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HRLConstructionManagerAPI.Services;

public sealed class AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public AuthResponse Login(LoginInput input)
    {
        var user = userRepository.GetByMobileNumber(input.MobileNumber);
        if (user is null || user.Password != input.Password)
        {
            throw new UnauthorizedAccessException("Invalid mobile number or password.");
        }

        if (!user.Enable)
        {
            throw new UnauthorizedAccessException("User is inactive.");
        }

        var expiresOn = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
        var token = GenerateToken(user, expiresOn);

        return new AuthResponse(token, expiresOn, ToDto(user));
    }

    private string GenerateToken(User user, DateTime expiresOn)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.MobileNumber),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.MobilePhone, user.MobileNumber),
            new(ClaimTypes.Name, user.Name),
            new("roleId", user.RoleId.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.Role?.RoleName))
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresOn,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserDto ToDto(User user) =>
        new(
            user.Id,
            user.MobileNumber,
            user.Name,
            user.RoleId,
            user.Address,
            user.Email,
            user.Password,
            user.Enable,
            user.CreatedOn,
            user.CreatedBy,
            user.ModifiedOn,
            user.ModifiedBy);
}
