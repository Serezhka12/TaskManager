using TaskManager.Data;
using TaskManager.Entities;

namespace TaskManager.Repositories;

public class UserTasksRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public List<UserTask> GetUserTasks(int userId)
    {
        var users = _dataStorage.GetUserTasks();
        var tasks = _dataStorage.GetTasks();
        _ = users.TryGetValue(userId, out var userTasksId);

        var result = new List<UserTask>();
        if (userTasksId == null)
        {
            return result;
        }

        foreach (var taskId in userTasksId)
        {
            tasks.TryGetValue(taskId, out var task);
            if (task != null) result.Add(task);
        }

        return result;
    }

    public bool AssignTaskToUser(int userId, int taskId)
    {
        var allTasks = _dataStorage.GetUserTasks();
        var exist = allTasks.TryGetValue(userId, out var tasks);

        if (!exist || tasks == null)
        {
            return allTasks.TryAdd(userId, [taskId]);
        }

        tasks.Add(taskId);

        return true;
    }

    public bool UnAssignAllUserTasks(int userId)
    {
        var userTasks = _dataStorage.GetUserTasks();
        return userTasks.TryRemove(userId, out _);
    }

    public void UnAssignUserTasks(int userId, int taskId)
    {
        var allTasks = _dataStorage.GetUserTasks();
        _ = allTasks.TryGetValue(userId, out var tasks);
        tasks?.Remove(taskId);
    }

    public UserTask? GetUserTaskById(int userId, int taskId)
    {
        var tasks = GetUserTasks(userId);
        return tasks.FirstOrDefault(t => t.Id == taskId);
    }


    private static UserTasksRepository? _instance;
    private UserTasksRepository(){}

    public static UserTasksRepository GetInstance()
    {
        if (_instance == null)
        {
            _instance = new UserTasksRepository();
        }

        return _instance;
    }
}