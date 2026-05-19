using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface IUserRepository
{
    IReadOnlyCollection<User> GetAll();

    User? GetById(int id);

    User? GetByMobileNumber(string mobileNumber);

    User Add(User user);

    User? Update(User user);
}
