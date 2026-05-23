using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class UserService(IUserRepository userRepository, IRoleRepository roleRepository) : IUserService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public IReadOnlyCollection<UserDto> GetUsers() => userRepository.GetAll().Select(ToDto).ToArray();

    public UserPage GetUsersPage(int pageNumber, int pageSize, string? search)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = userRepository.GetPaged(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new UserPage(
            items.Select(ToDto).ToArray(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    public UserDto? GetUser(int id)
    {
        var user = userRepository.GetById(id);

        return user is null ? null : ToDto(user);
    }

    public UserDto CreateUser(CreateUserInput input)
    {
        var mobileNumber = input.MobileNumber.Trim();
        EnsureMobileNumberUnique(mobileNumber);

        var user = BuildUser(mobileNumber, input.Name, input.RoleId, input.Address, input.Email, input.Enable);
        user.Password = input.Password?.Trim() ?? string.Empty;
        user.CreatedBy = input.CreatedBy;

        return ToDto(userRepository.Add(user));
    }

    public UserDto? UpdateUser(UpdateUserInput input)
    {
        EnsureUserExists(input.Id);

        var mobileNumber = input.MobileNumber.Trim();
        EnsureMobileNumberUnique(mobileNumber, input.Id);

        var user = BuildUser(mobileNumber, input.Name, input.RoleId, input.Address, input.Email, input.Enable);
        user.Id = input.Id;
        user.Password = input.Password?.Trim() ?? string.Empty;
        user.ModifiedBy = input.ModifiedBy;

        var updatedUser = userRepository.Update(user);
        return updatedUser is null ? null : ToDto(updatedUser);
    }

    private static User BuildUser(
        string mobileNumber, string name, int roleId,
        string address, string? email, bool enable) =>
        new()
        {
            MobileNumber = mobileNumber,
            Name = name.Trim(),
            RoleId = roleId,
            Address = address.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            Enable = enable
        };


    private void EnsureUserExists(int userId)
    {
        if (userRepository.GetById(userId) is null)
        {
            throw new GraphQLException($"User with id '{userId}' was not found.");
        }
    }

    private void EnsureMobileNumberUnique(string mobileNumber, int? userId = null)
    {
        var normalizedMobile = mobileNumber.Trim();
        var existingUser = userRepository.GetByMobileNumber(normalizedMobile);
        if (existingUser is not null && existingUser.Id != userId)
        {
            throw new GraphQLException("Mobile number is already in use.");
        }
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
