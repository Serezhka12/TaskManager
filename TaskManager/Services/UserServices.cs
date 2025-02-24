using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class UserServices
{
    private readonly UserRepository _userRepository = UserRepository.GetInstance();

    public OperationResult<bool> CreateUser(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Developer
        };
        return _userRepository.AddUser(user) ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotCreateDeveloper());
    }

    public OperationResult<bool> CreateManager(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Manager
        };
        return _userRepository.AddUser(user) ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotCreateManager());
    }
}