using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
namespace TaskManager.Repositories;

public class UserRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public User? GetUser(int userId)
    {
        var users = _dataStorage.GetUsers();
        var exist = users.TryGetValue(userId, out var user);
        return exist ? user : null;
    }

    public bool DeleteUser(int userId)
    {
        var users = _dataStorage.GetUsers();
        return users.Remove(userId, out _);
    }

    public int? AddUser(UserDto userDto)
    {
        var users = _dataStorage.GetUsers();
        var id = _dataStorage.GetUserId();
        var user = new User()
        {
            Id = id,
            Name = userDto.Name,
            Role = userDto.Role
        };
        return users.TryAdd(user.Id, user) ? user.Id : null;
    }

    public List<User> GetAllUsers()
    {
        return _dataStorage.GetUsers().Values.ToList();
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