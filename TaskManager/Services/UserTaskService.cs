using TaskManager.Entities;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class UserTaskService
{
    private readonly UserRepository _userRepository = UserRepository.GetInstance();
    private readonly UserTasksRepository _userTasksRepository = UserTasksRepository.GetInstance();
    private readonly TaskRepository _taskRepository = TaskRepository.GetInstance();

    public OperationResult<List<UserTask>> GetUserAllTasks(int userId)
    {
        var user = _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<List<UserTask>>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var tasks = _userTasksRepository.GetUserTasks(userId);

        return OperationResult<List<UserTask>>.Success(tasks);
    }

    public OperationResult<List<UserTask>> GetUserTasksByActive(int userId, bool isActive)
    {
        var user = _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<List<UserTask>>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var tasks = _userTasksRepository.GetUserTasks(userId);

        return OperationResult<List<UserTask>>.Success(tasks.Where(t => t.IsActive == isActive).ToList());
    }

    public OperationResult<bool> AssignTaskToUser(int userId, int taskId)
    {
        var user = _userRepository.GetUser(userId);
        if (user == null)
        {
            return OperationResult<bool>.Failure(ErrorMessages.UserNotFound(userId));
        }

        var success = _userTasksRepository.AssignTaskToUser(userId, taskId);
        return success ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotAssignTask());
    }

    public async Task CompleteTask(int taskId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        if (task == null)
        {
            return;
        }
        await Task.Run(() => Thread.Sleep(3000));  // imitating work
        _taskRepository.ChangeTaskStatus(taskId, false);
    }
}