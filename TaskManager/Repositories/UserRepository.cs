using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Interfaces;

namespace TaskManager.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public async Task<User?> GetUser(int userId)
    {
        var users = await _dataStorage.GetUsers();
        return users.TryGetValue(userId, out var user) ? user : null;
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var users = await _dataStorage.GetUsers();
        var result = users.Remove(userId);
        if (result)
        {
            await _dataStorage.SaveUsers(users);
        }
        return result;
    }

    public async Task<int?> AddUser(UserDto userDto)
    {
        var users = await _dataStorage.GetUsers();
        var id = await _dataStorage.GetUserId();
        var user = new User()
        {
            Id = id,
            Name = userDto.Name,
            Role = userDto.Role
        };

        if (users.TryAdd(user.Id, user))
        {
            await _dataStorage.SaveUsers(users);
            return user.Id;
        }
        return null;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await _dataStorage.GetUsers();
        return users.Values.ToList();
    }

    private static UserRepository? _instance;
    private UserRepository(){}

    public static UserRepository GetInstance()
    {
        if (_instance == null)
        {
            _instance = new UserRepository();
        }

        return _instance;
    }
}