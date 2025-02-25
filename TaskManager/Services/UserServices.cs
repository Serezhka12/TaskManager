using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Interfaces;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class UserServices: IUserServices
{
    private readonly IUserRepository _userRepository = UserRepository.GetInstance();

    public async Task<OperationResult<int?>> CreateUser(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Developer
        };
        var userId = await _userRepository.AddUser(user);
        return userId == null
            ? OperationResult<int?>.Failure(ErrorMessages.CanNotCreateDeveloper())
            : OperationResult<int?>.Success(userId);
    }

    public async Task<OperationResult<int?>> CreateManager(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Manager
        };
        var userId = await _userRepository.AddUser(user);
        return userId == null
            ? OperationResult<int?>.Failure(ErrorMessages.CanNotCreateManager())
            : OperationResult<int?>.Success(userId);
    }

    public async Task<OperationResult<User?>> GetUserById(int userId)
    {
        var user = await _userRepository.GetUser(userId);
        return user != null ?
            OperationResult<User?>.Success(user) :
            OperationResult<User?>.Failure(ErrorMessages.UserNotFound(userId));
    }

    public async Task<OperationResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return OperationResult<List<User>>.Success(users);
    }

    public async Task<OperationResult<List<User>>> GetAllDevelopers()
    {
        var users = await _userRepository.GetAllUsers();
        var developers = users.Where(u => u.Role == Role.Developer).ToList();
        return OperationResult<List<User>>.Success(developers);
    }
}