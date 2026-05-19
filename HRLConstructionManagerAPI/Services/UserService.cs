using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class UserService(IUserRepository userRepository, IRoleRepository roleRepository) : IUserService
{
    public IReadOnlyCollection<UserDto> GetUsers() => userRepository.GetAll().Select(ToDto).ToArray();

    public UserDto? GetUser(int id)
    {
        var user = userRepository.GetById(id);

        return user is null ? null : ToDto(user);
    }

    public UserDto CreateUser(CreateUserInput input)
    {
        EnsureRoleExists(input.RoleId);

        var user = new User
        {
            MobileNumber = input.MobileNumber,
            Name = input.Name,
            Password = input.Password,
            RoleId = input.RoleId,
            Address = input.Address,
            Email = input.Email,
            CreatedBy = input.CreatedBy
        };

        return ToDto(userRepository.Add(user));
    }

    public UserDto? UpdateUser(UpdateUserInput input)
    {
        EnsureRoleExists(input.RoleId);

        var user = new User
        {
            Id = input.Id,
            MobileNumber = input.MobileNumber,
            Name = input.Name,
            RoleId = input.RoleId,
            Address = input.Address,
            Email = input.Email,
            ModifiedBy = input.ModifiedBy
        };

        var updatedUser = userRepository.Update(user);

        return updatedUser is null ? null : ToDto(updatedUser);
    }

    private void EnsureRoleExists(int roleId)
    {
        if (roleRepository.GetById(roleId) is null)
        {
            throw new GraphQLException($"Role with id '{roleId}' was not found.");
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
            user.CreatedOn,
            user.CreatedBy,
            user.ModifiedOn,
            user.ModifiedBy);
}
