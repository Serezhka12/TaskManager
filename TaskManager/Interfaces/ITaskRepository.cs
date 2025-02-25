using TaskManager.Dto;
using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface ITaskRepository
{
    Task<UserTask?> GetTaskById(int taskId);
    Task<List<UserTask>> GetAllTasks();
    Task<bool> CreateTask(UserTask userTask);
    Task UpdateTask(int taskId, TaskDto newTask);
    Task<bool> DeleteTask(int taskId);
    Task<bool> ChangeTaskStatus(int taskId, bool isActive);
}