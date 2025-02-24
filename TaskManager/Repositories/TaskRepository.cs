using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;

namespace TaskManager.Repositories;

public class TaskRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public UserTask? GetTaskById(int taskId)
    {
        _ = _dataStorage.GetTasks().TryGetValue(taskId, out var result);
        return result;
    }

    public List<UserTask> GetAllTasks()
    {
        return _dataStorage.GetTasks().Select(kvp => kvp.Value).ToList();
    }

    public bool CreateTask(UserTask userTask)
    {
        var tasks = _dataStorage.GetTasks();
        var taskId = _dataStorage.GetTaskId();
        userTask.Id = taskId;
        return tasks.TryAdd(userTask.Id, userTask);
    }

    public void UpdateTask(int taskId, TaskDto newTask)
    {
        var tasks = _dataStorage.GetTasks();
        _ = tasks.TryGetValue(taskId, out var task);

        task!.Name = newTask.Name;
        if (newTask.Description != null)
        {
            task.Description = newTask.Description;
        }
    }

    public bool DeleteTask(int taskId)
    {
        var tasks = _dataStorage.GetTasks();
        return tasks.TryRemove(taskId, out _);
    }

    public bool ChangeTaskStatus(int taskId, bool isActive)
    {
        var tasks = _dataStorage.GetTasks();
        _ = tasks.TryGetValue(taskId, out var task);
        if (task == null)
        {
            return false;
        }

        task.IsActive = isActive;
        return true;
    }

    private static TaskRepository? _instance;
    private TaskRepository(){}

    public static TaskRepository GetInstance()
    {
        if (_instance == null)
        {
            _instance = new TaskRepository();
        }

        return _instance;
    }
}