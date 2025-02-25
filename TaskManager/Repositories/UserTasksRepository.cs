using TaskManager.Data;
using TaskManager.Entities;
using TaskManager.Interfaces;

namespace TaskManager.Repositories;

public class UserTasksRepository: IUserTasksRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public async Task<List<UserTask>> GetUserTasks(int userId)
    {
        var users = await _dataStorage.GetUserTasks();
        var tasks = await _dataStorage.GetTasks();
        _ = users.TryGetValue(userId, out var userTasksId);

        var result = new List<UserTask>();
        if (userTasksId == null)
        {
            return result;
        }

        foreach (var taskId in userTasksId)
        {
            if (tasks.TryGetValue(taskId, out var task))
            {
                result.Add(task);
            }
        }

        return result;
    }

    public async Task<bool> AssignTaskToUser(int userId, int taskId)
    {
        var allTasks = await _dataStorage.GetUserTasks();
        var exists = allTasks.TryGetValue(userId, out var tasks);

        if (!exists || tasks == null)
        {
            allTasks[userId] = [taskId];
            await _dataStorage.SaveUserTasks(allTasks);
            return true;
        }

        tasks.Add(taskId);
        await _dataStorage.SaveUserTasks(allTasks);
        return true;
    }

    public async Task<bool> UnAssignAllUserTasks(int userId)
    {
        var userTasks = await _dataStorage.GetUserTasks();
        var result = userTasks.Remove(userId);
        if (result)
        {
            await _dataStorage.SaveUserTasks(userTasks);
        }
        return result;
    }

    public async Task UnAssignUserTasks(int userId, int taskId)
    {
        var allTasks = await _dataStorage.GetUserTasks();
        if (allTasks.TryGetValue(userId, out var tasks))
        {
            tasks?.Remove(taskId);
            await _dataStorage.SaveUserTasks(allTasks);
        }
    }

    public async Task<UserTask?> GetUserTaskById(int userId, int taskId)
    {
        var tasks = await GetUserTasks(userId);
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