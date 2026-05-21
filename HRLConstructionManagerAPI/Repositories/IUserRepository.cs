using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface IUserRepository
{
    IReadOnlyCollection<User> GetAll();

    (IReadOnlyCollection<User> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search);

    User? GetById(int id);

    User? GetByMobileNumber(string mobileNumber);

    User Add(User user);

    User? Update(User user);
}
