using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class UserManagementQuery
{
    [Authorize]
    public IReadOnlyCollection<Role> GetRoles([Service] IRoleService roleService) =>
        roleService.GetRoles();

    [Authorize]
    public Role? GetRole(int id, [Service] IRoleService roleService) =>
        roleService.GetRole(id);

    [Authorize]
    public IReadOnlyCollection<UserDto> GetUsers([Service] IUserService userService) =>
        userService.GetUsers();

    [Authorize]
    public UserDto? GetUser(int id, [Service] IUserService userService) =>
        userService.GetUser(id);
}
