using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfRoleRepository(AppDbContext dbContext) : IRoleRepository
{
    public IReadOnlyCollection<Role> GetAll() =>
        dbContext.Roles
            .AsNoTracking()
            .OrderBy(role => role.Id)
            .ToArray();

    public Role? GetById(int id) =>
        dbContext.Roles
            .AsNoTracking()
            .FirstOrDefault(role => role.Id == id);

    public Role Add(Role role)
    {
        dbContext.Roles.Add(role);
        dbContext.SaveChanges();

        return role;
    }

    public Role? Update(Role role)
    {
        var existingRole = dbContext.Roles.FirstOrDefault(currentRole => currentRole.Id == role.Id);
        if (existingRole is null)
        {
            return null;
        }

        existingRole.RoleName = role.RoleName;
        existingRole.Enable = role.Enable;
        existingRole.ModifiedOn = DateTime.UtcNow;

        dbContext.SaveChanges();

        return existingRole;
    }
}
