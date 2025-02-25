using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface IUserServices
{
    Task<OperationResult<int?>> CreateUser(string name);
    Task<OperationResult<int?>> CreateManager(string name);
    Task<OperationResult<User?>> GetUserById(int userId);
    Task<OperationResult<List<User>>> GetAllUsers();
    Task<OperationResult<List<User>>> GetAllDevelopers();
}