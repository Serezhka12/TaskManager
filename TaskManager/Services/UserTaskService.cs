using TaskManager.Data;
using TaskManager.Entities;
using TaskManager.Interfaces;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class UserTaskService: IUserTaskService
{
    private readonly IUserRepository _userRepository = UserRepository.GetInstance();
    private readonly IUserTasksRepository _userTasksRepository = UserTasksRepository.GetInstance();
    private readonly ITaskRepository _taskRepository = TaskRepository.GetInstance();

    public async Task<OperationResult<List<UserTask>>> GetUserAllTasks(int userId)
    {
        var user = await _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<List<UserTask>>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var tasks = await _userTasksRepository.GetUserTasks(userId);
        return OperationResult<List<UserTask>>.Success(tasks);
    }

    public async Task<OperationResult<List<UserTask>>> GetUserTasksByActive(int userId, bool isActive)
    {
        var user = await _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<List<UserTask>>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var tasks = await _userTasksRepository.GetUserTasks(userId);
        return OperationResult<List<UserTask>>.Success(tasks.Where(t => t.IsActive == isActive).ToList());
    }

    public async Task<OperationResult<bool>> AssignTaskToUser(int userId, int taskId)
    {
        var user = await _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<bool>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var success = await _userTasksRepository.AssignTaskToUser(userId, taskId);
        return success ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotAssignTask());
    }

    public async Task CompleteTask(int taskId)
    {
        var task = await _taskRepository.GetTaskById(taskId);
        if (task == null)
        {
            return;
        }
        await Task.Delay(500);
        await _taskRepository.ChangeTaskStatus(taskId, false);
    }
}