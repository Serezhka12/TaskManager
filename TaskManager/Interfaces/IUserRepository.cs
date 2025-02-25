using TaskManager.Dto;
using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUser(int userId);
    Task<bool> DeleteUser(int userId);
    Task<int?> AddUser(UserDto userDto);
    Task<List<User>> GetAllUsers();
}
