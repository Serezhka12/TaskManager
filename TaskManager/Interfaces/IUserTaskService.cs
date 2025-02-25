using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface IUserTaskService
{
    Task<OperationResult<List<UserTask>>> GetUserAllTasks(int userId);
    Task<OperationResult<List<UserTask>>> GetUserTasksByActive(int userId, bool isActive);
    Task<OperationResult<bool>> AssignTaskToUser(int userId, int taskId);
    Task CompleteTask(int taskId);
}