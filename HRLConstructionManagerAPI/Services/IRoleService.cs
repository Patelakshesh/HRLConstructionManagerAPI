using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface IRoleService
{
    IReadOnlyCollection<Role> GetRoles();

    Role? GetRole(int id);

    Role CreateRole(CreateRoleInput input);

    Role? UpdateRole(UpdateRoleInput input);
}
