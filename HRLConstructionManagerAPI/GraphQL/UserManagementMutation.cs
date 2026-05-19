using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class UserManagementMutation
{
    [Authorize]
    public Role CreateRole(CreateRoleInput input, [Service] IRoleService roleService) =>
        roleService.CreateRole(input);

    [Authorize]
    public Role? UpdateRole(UpdateRoleInput input, [Service] IRoleService roleService) =>
        roleService.UpdateRole(input);

    [Authorize]
    public UserDto CreateUser(CreateUserInput input, [Service] IUserService userService) =>
        userService.CreateUser(input);

    [Authorize]
    public UserDto? UpdateUser(UpdateUserInput input, [Service] IUserService userService) =>
        userService.UpdateUser(input);
}
