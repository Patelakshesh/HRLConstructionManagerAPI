using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface IRoleRepository
{
    IReadOnlyCollection<Role> GetAll();

    Role? GetById(int id);

    Role Add(Role role);

    Role? Update(Role role);
}
