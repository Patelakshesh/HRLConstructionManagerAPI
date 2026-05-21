using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfUserRepository(AppDbContext dbContext) : IUserRepository
{
    public IReadOnlyCollection<User> GetAll() =>
        dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.Id)
            .ToArray();

    public (IReadOnlyCollection<User> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search)
    {
        var query = dbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(user =>
                user.Name.Contains(term) ||
                user.MobileNumber.Contains(term) ||
                (user.Email != null && user.Email.Contains(term)));
        }

        var totalCount = query.Count();
        var items = query
            .OrderBy(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return (items, totalCount);
    }

    public User? GetById(int id) =>
        dbContext.Users
            .AsNoTracking()
            .FirstOrDefault(user => user.Id == id);

    public User? GetByMobileNumber(string mobileNumber) =>
        dbContext.Users
            .Include(user => user.Role)
            .AsNoTracking()
            .FirstOrDefault(user => user.MobileNumber == mobileNumber);

    public User Add(User user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        return user;
    }

    public User? Update(User user)
    {
        var existingUser = dbContext.Users.FirstOrDefault(currentUser => currentUser.Id == user.Id);
        if (existingUser is null)
        {
            return null;
        }

        existingUser.MobileNumber = user.MobileNumber;
        existingUser.Name = user.Name;
        existingUser.RoleId = user.RoleId;
        existingUser.Address = user.Address;
        existingUser.Email = user.Email;
        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            existingUser.Password = user.Password;
        }
        existingUser.Enable = user.Enable;
        existingUser.ModifiedOn = DateTime.UtcNow;
        existingUser.ModifiedBy = user.ModifiedBy;

        dbContext.SaveChanges();

        return existingUser;
    }
}
