using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface IUserTasksRepository
{
    Task<List<UserTask>> GetUserTasks(int userId);
    Task<bool> AssignTaskToUser(int userId, int taskId);
    Task<bool> UnAssignAllUserTasks(int userId);
    Task UnAssignUserTasks(int userId, int taskId);
    Task<UserTask?> GetUserTaskById(int userId, int taskId);
}