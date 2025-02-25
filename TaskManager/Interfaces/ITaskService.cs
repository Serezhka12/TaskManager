using TaskManager.Dto;
using TaskManager.Entities;

namespace TaskManager.Interfaces;

public interface ITaskService
{
    Task<OperationResult<bool>> CreateTask(TaskDto taskDto);
    Task<OperationResult<bool>> UpdateTask(int taskId, TaskDto taskDto);
    Task<OperationResult<bool>> DeleteTask(int taskId);
    Task<OperationResult<List<UserTask>>> GetAllTasks();
    Task<OperationResult<UserTask?>> GetTasksById(int taskId);
}