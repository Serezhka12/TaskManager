using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class UserServices
{
    private readonly UserRepository _userRepository = UserRepository.GetInstance();

    public OperationResult<int?> CreateUser(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Developer
        };
        var userId = _userRepository.AddUser(user);
        return userId == null
            ? OperationResult<int?>.Failure(ErrorMessages.CanNotCreateDeveloper())
            : OperationResult<int?>.Success(userId);

    }

    public OperationResult<int?> CreateManager(string name)
    {
        var user = new UserDto()
        {
            Name = name,
            Role = Role.Manager
        };
        var userId = _userRepository.AddUser(user);
        return userId == null
            ? OperationResult<int?>.Failure(ErrorMessages.CanNotCreateManager())
            : OperationResult<int?>.Success(userId);
    }

    public OperationResult<User?> GetUserById(int userId)
    {
        var user = _userRepository.GetUser(userId);
        return user != null ?
            OperationResult<User?>.Success(user) :
            OperationResult<User?>.Failure(ErrorMessages.UserNotFound(userId));
    }

    public OperationResult<List<User>> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        return OperationResult<List<User>>.Success(users);
    }

    public OperationResult<List<User>> GetAllDevelopers()
    {
        var users = _userRepository.GetAllUsers();
        var developers = users.Where(u => u.Role == Role.Developer).ToList();
        return OperationResult<List<User>>.Success(developers);
    }
}