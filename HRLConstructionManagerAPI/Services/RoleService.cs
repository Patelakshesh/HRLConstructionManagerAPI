using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public IReadOnlyCollection<Role> GetRoles() => roleRepository.GetAll();

    public Role? GetRole(int id) => roleRepository.GetById(id);

    public Role CreateRole(CreateRoleInput input)
    {
        var role = new Role
        {
            RoleName = input.RoleName,
            Enable = input.Enable
        };

        return roleRepository.Add(role);
    }

    public Role? UpdateRole(UpdateRoleInput input)
    {
        var role = new Role
        {
            Id = input.Id,
            RoleName = input.RoleName,
            Enable = input.Enable
        };

        return roleRepository.Update(role);
    }
}
