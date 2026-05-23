using System.Net.Mail;
using System;
using System.Text.RegularExpressions;
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
        EnsureRoleExists(input.RoleId);

        var mobileNumber = input.MobileNumber.Trim();
        var name = input.Name.Trim();
        var address = input.Address.Trim();
        var email = string.IsNullOrWhiteSpace(input.Email) ? null : input.Email.Trim();
        var password = string.IsNullOrWhiteSpace(input.Password) ? null : input.Password.Trim();

        EnsureMobileNumberUnique(mobileNumber);

        var user = new User
        {
            MobileNumber = mobileNumber,
            Name = name,
            Password = password ?? input.Password,
            RoleId = input.RoleId,
            Address = address,
            Email = email,
            Enable = input.Enable,
            CreatedBy = input.CreatedBy
        };

        return ToDto(userRepository.Add(user));
    }

    public UserDto? UpdateUser(UpdateUserInput input)
    {
        EnsureRoleExists(input.RoleId);
        EnsureUserExists(input.Id);

        var mobileNumber = input.MobileNumber.Trim();
        var name = input.Name.Trim();
        var address = input.Address.Trim();
        var email = string.IsNullOrWhiteSpace(input.Email) ? null : input.Email.Trim();
        var password = string.IsNullOrWhiteSpace(input.Password) ? null : input.Password.Trim();

        EnsureMobileNumberUnique(mobileNumber, input.Id);

        var user = new User
        {
            Id = input.Id,
            MobileNumber = mobileNumber,
            Name = name,
            RoleId = input.RoleId,
            Address = address,
            Email = email,
            Password = password ?? string.Empty,
            Enable = input.Enable,
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

    private static bool IsValidMobileNumber(string mobileNumber) =>
        Regex.IsMatch(mobileNumber.Trim(), "^\\d{10}$");

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new MailAddress(email);
            return address.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
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
            user.Enable,
            user.CreatedOn,
            user.CreatedBy,
            user.ModifiedOn,
            user.ModifiedBy);
}
