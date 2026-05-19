using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface IUserService
{
    IReadOnlyCollection<UserDto> GetUsers();

    UserDto? GetUser(int id);

    UserDto CreateUser(CreateUserInput input);

    UserDto? UpdateUser(UpdateUserInput input);
}
