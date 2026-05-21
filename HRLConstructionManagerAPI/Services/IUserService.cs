using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface IUserService
{
    IReadOnlyCollection<UserDto> GetUsers();

    UserPage GetUsersPage(int pageNumber, int pageSize, string? search);

    UserDto? GetUser(int id);

    UserDto CreateUser(CreateUserInput input);

    UserDto? UpdateUser(UpdateUserInput input);
}
