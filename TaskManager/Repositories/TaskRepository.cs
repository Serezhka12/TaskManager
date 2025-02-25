using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Interfaces;

namespace TaskManager.Repositories;

public class TaskRepository: ITaskRepository
{
    private readonly DataStorage _dataStorage = DataStorage.GetInstance();

    public async Task<UserTask?> GetTaskById(int taskId)
    {
        var tasks = await _dataStorage.GetTasks();
        return tasks.GetValueOrDefault(taskId);
    }

    public async Task<List<UserTask>> GetAllTasks()
    {
        var tasks = await _dataStorage.GetTasks();
        return tasks.Values.ToList();
    }

    public async Task<bool> CreateTask(UserTask userTask)
    {
        var tasks = await _dataStorage.GetTasks();
        var taskId = await _dataStorage.GetTaskId();
        userTask.Id = taskId;

        if (!tasks.TryAdd(userTask.Id, userTask)) return false;
        await _dataStorage.SaveTasks(tasks);
        return true;
    }

    public async Task UpdateTask(int taskId, TaskDto newTask)
    {
        var tasks = await _dataStorage.GetTasks();
        if (tasks.TryGetValue(taskId, out var task))
        {
            task.Name = newTask.Name;
            if (newTask.Description != null)
            {
                task.Description = newTask.Description;
            }
            await _dataStorage.SaveTasks(tasks);
        }
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        var tasks = await _dataStorage.GetTasks();
        var result = tasks.Remove(taskId);
        if (result)
        {
            await _dataStorage.SaveTasks(tasks);
        }
        return result;
    }

    public async Task<bool> ChangeTaskStatus(int taskId, bool isActive)
    {
        var tasks = await _dataStorage.GetTasks();
        if (!tasks.TryGetValue(taskId, out var task)) return false;
        task.IsActive = isActive;
        await _dataStorage.SaveTasks(tasks);
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